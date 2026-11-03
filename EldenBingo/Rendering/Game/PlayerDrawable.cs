using EldenBingo.GameInterop;
using EldenBingo.Util;
using EldenBingoCommon;
using SFML.Graphics;
using SFML.System;

namespace EldenBingo.Rendering.Game
{
    public class PlayerDrawable : IDrawable, IUpdateable
    {
        private static readonly Texture _playerTexture;
        private static readonly Texture _playerIconTexture;
        private static readonly Sprite _playerArrowSprite;
        private static readonly Sprite _playerIconSprite;
        private static readonly Shader? _shader;
        private static readonly SFML.Graphics.Font Font;

        private float _targetX, _targetY, _targetAngle, _previousX, _previousY, _previousAngle, _interpTime;
        private float _timeLeftToInterpolate = 0f;

        private const int MaxTrailPoints = 36;
        private static readonly TimeSpan TrailLifetime = TimeSpan.FromSeconds(35);
        private readonly List<TrailSample> _trail = new List<TrailSample>();
        private readonly VertexArray _trailVertices = new VertexArray(PrimitiveType.LineStrip);
        private readonly Text _staleAgeTag;
        private DateTime _lastCoordinateAt = DateTime.MinValue;
        private int _lastAgeLabelSeconds = -1;

        private readonly record struct TrailSample(float X, float Y, MapInstance Map, DateTime Time);


        private MapWindow _window;
        private ICoordinateProvider _coordinateProvider;
        private RoundTableDrawable _roundTable;

        static PlayerDrawable()
        {
            Font = new SFML.Graphics.Font("LibraSans.ttf");

            _playerTexture = new Texture("./Textures/player.png");
            _playerTexture.Smooth = true;
            _playerArrowSprite = new Sprite(_playerTexture);
            _playerArrowSprite.Origin = new Vector2f(33f, 86f);
            _playerIconTexture = new Texture("./Textures/player-icon.png");
            _playerIconTexture.Smooth = true;
            _playerIconSprite = new Sprite(_playerIconTexture);
            _playerIconSprite.Origin = new Vector2f(34f, 34f);

            try
            {
                _shader = SpriteShader.Create();
            }
            catch (Exception)
            {
                _shader = null;
            }
        }

        public PlayerDrawable(MapWindow window, ICoordinateProvider provider, RoundTableDrawable roundTable)
        {
            _window = window;
            _coordinateProvider = provider;
            _roundTable = roundTable;
            _staleAgeTag = new Text(string.Empty, Font, 30) { OutlineColor = SFML.Graphics.Color.Black, OutlineThickness = 3f };
            if (provider.MapCoordinates.HasValue)
            {
                var c = provider.MapCoordinates.Value;
                RecordTrailPoint(c.X, c.Y, c.MapInstance);
                setNewTarget(c.X, c.Y, c.Angle, c.IsUnderground, c.MapInstance, 0f);
            }
            else
            {
                ValidPosition = false;
            }
            Name = provider.Name;
            if (!string.IsNullOrWhiteSpace(Name))
                NameTag = new Text(Name, Font, 42) { OutlineColor = SFML.Graphics.Color.Black, OutlineThickness = 3f };
        }

        public bool Enabled { get; set; } = true;
        public bool Visible { get; set; } = true;
        public Guid Guid => _coordinateProvider.Guid;

        public float Angle { get; private set; }
        public string Name { get; private set; }
        public Text? NameTag { get; private set; } = null;
        public bool Underground { get; private set; }
        public bool ValidPosition { get; private set; }
        public MapInstance MapInstance { get; private set; }
        public float X { get; private set; }
        public float Y { get; private set; }

        public static void DisposeStatic()
        {
            _playerArrowSprite.Dispose();
            _playerIconSprite.Dispose();
            _playerTexture.Dispose();
            _playerIconTexture.Dispose();
            _shader?.Dispose();
        }

        public void Update(float dt)
        {
            if (_coordinateProvider.Changed)
            {
                var newCoords = _coordinateProvider.MapCoordinates;
                if (newCoords.HasValue && newCoords.Value.X > 0 && newCoords.Value.Y > 0)
                {
                    RecordTrailPoint(newCoords.Value.X, newCoords.Value.Y, newCoords.Value.MapInstance);
                    setNewTarget(newCoords.Value.X, newCoords.Value.Y, newCoords.Value.Angle, newCoords.Value.IsUnderground, newCoords.Value.MapInstance, 0.1f);
                }
                else
                {
                    ValidPosition = false;
                    ClearTrail();
                }
            }
            PruneTrail(DateTime.UtcNow);
            _timeLeftToInterpolate = Math.Max(0, _timeLeftToInterpolate - dt);
            if (_interpTime > 0 && _timeLeftToInterpolate > 0)
            {
                var t = 1f - _timeLeftToInterpolate / _interpTime;
                X = Math2.Lerp(_previousX, _targetX, t);
                Y = Math2.Lerp(_previousY, _targetY, t);
                Angle = Math2.LerpAngle(_previousAngle, _targetAngle, t);
            }
            else
            {
                X = _targetX;
                Y = _targetY;
                Angle = _targetAngle;
            }
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            if (!ValidPosition)
                return;

            var pos = GetConvertedPosition();
            if (_window.ShowPlayerTrails)
                DrawTrailAndAge(target, pos);

            var trans = Transform.Identity;
            trans.Translate(pos);
            trans.Rotate(Angle);

            var cameraScaleAdjust = (float)Math.Pow(_window.Camera.Zoom, 0.85); 
            //Initial scale, scaled up to match window size
            var scale = 0.62f * cameraScaleAdjust;
            trans.Scale(scale, scale);
            var st = new RenderStates(BlendMode.Alpha, trans, _playerArrowSprite.Texture, _shader);

            var color = new SFML.Graphics.Glsl.Vec4(_coordinateProvider.Color.R, _coordinateProvider.Color.G, _coordinateProvider.Color.B, _coordinateProvider.Color.A);
            _shader?.SetUniform("alpha", Underground ? 0.4f : 1.0f);
            _shader?.SetUniform("tint", color);

            _window.Draw(_playerArrowSprite, st);

            trans = Transform.Identity;
            trans.Translate(pos);
            trans.Scale(scale, scale);
            st = new RenderStates(BlendMode.Alpha, trans, _playerIconSprite.Texture, _shader);
            _window.Draw(_playerIconSprite, st);

            if (_window.ShowPlayerNames && NameTag != null)
            {
                var st2 = RenderStates.Default;
                var trans2 = Transform.Identity;
                var scale2 = 0.5f * cameraScaleAdjust;
                var bounds = NameTag.GetLocalBounds();
                var width = bounds.Width * scale2;
                var height = 40f * scale2;
                trans2.Translate(new Vector2f(pos.X - width * 0.5f, pos.Y + height));
                trans2.Scale(scale2, scale2);
                st2.Transform = trans2;
                _window.Draw(NameTag, st2);
            }
        }

        public Vector2f GetConvertedPosition()
        {
            var pos = new Vector2f(X, Y);
            if (MapInstance == MapInstance.MainMap && MapWindow.RoundTableRectangle.Contains(new PointF(X, Y)))
            {
                return _roundTable.Position;
            }
            return pos;
        }

        public void Dispose()
        {
            NameTag?.Dispose();
            _staleAgeTag.Dispose();
            _trailVertices.Dispose();
        }

        public void ClearTrail()
        {
            _trail.Clear();
            _lastCoordinateAt = DateTime.MinValue;
            _lastAgeLabelSeconds = -1;
            _trailVertices.Clear();
        }

        private void PruneTrail(DateTime now)
        {
            while (_trail.Count > 0 && now - _trail[0].Time > TrailLifetime)
                _trail.RemoveAt(0);
        }

        private void RecordTrailPoint(float x, float y, MapInstance map)
        {
            var now = DateTime.UtcNow;
            _lastCoordinateAt = now;
            PruneTrail(now);
            if (_trail.Count > 0)
            {
                var previous = _trail[_trail.Count - 1];
                var distanceSq = Math2.DistSqr(previous.X, previous.Y, x, y);
                // Do not draw a line across map layers or teleports.
                if (previous.Map != map || distanceSq > 1000000f)
                    _trail.Clear();
                else if (distanceSq < 144f || (now - previous.Time).TotalMilliseconds < 250)
                    return;
            }
            _trail.Add(new TrailSample(x, y, map, now));
            if (_trail.Count > MaxTrailPoints)
                _trail.RemoveAt(0);
        }

        private void DrawTrailAndAge(RenderTarget target, Vector2f position)
        {
            var now = DateTime.UtcNow;
            if (_trail.Count >= 2)
            {
                _trailVertices.Clear();
                var baseColor = _coordinateProvider.Color;
                foreach (var point in _trail)
                {
                    var fractionRemaining = Math.Clamp(1f - (float)(now - point.Time).TotalSeconds / 35f, 0f, 1f);
                    var alpha = (byte)(145f * fractionRemaining);
                    Vector2f trailPos = point.Map == MapInstance.MainMap
                        && MapWindow.RoundTableRectangle.Contains(new PointF(point.X, point.Y))
                        ? _roundTable.Position : new Vector2f(point.X, point.Y);
                    _trailVertices.Append(new Vertex(trailPos,
                        new SFML.Graphics.Color(baseColor.R, baseColor.G, baseColor.B, alpha)));
                }
                target.Draw(_trailVertices, RenderStates.Default);
            }

            if (_lastCoordinateAt == DateTime.MinValue)
                return;
            int ageSeconds = (int)Math.Min(int.MaxValue, Math.Max(0, (now - _lastCoordinateAt).TotalSeconds));
            if (ageSeconds < 8)
                return;
            if (ageSeconds != _lastAgeLabelSeconds)
            {
                _staleAgeTag.DisplayedString = $"{ageSeconds}s";
                _lastAgeLabelSeconds = ageSeconds;
            }
            var scale = 0.4f * (float)Math.Pow(_window.Camera.Zoom, 0.85);
            var transform = Transform.Identity;
            transform.Translate(new Vector2f(position.X + 22f * scale, position.Y - 75f * scale));
            transform.Scale(scale, scale);
            var states = RenderStates.Default;
            states.Transform = transform;
            target.Draw(_staleAgeTag, states);
        }

        private void setNewTarget(float x, float y, float angle, bool underground, MapInstance map, float interpolationTime)
        {
            _previousX = X;
            _previousY = Y;
            _previousAngle = Angle;
            _targetX = x;
            _targetY = y;
            _targetAngle = angle;

            if (!ValidPosition)
            {
                X = _targetX;
                Y = _targetY;
                Angle = _targetAngle;
                MapInstance = map;
                ValidPosition = true;
                return;
            }
            ValidPosition = true;
            if (Math2.DistSqr(_previousX, _previousY, _targetX, _targetY) > 100)
            {
                //Distance > 10 -> Teleport instead of interpolate
                _interpTime = 0;
                _timeLeftToInterpolate = 0;
            }
            else
            {
                _interpTime = interpolationTime;
                _timeLeftToInterpolate = interpolationTime;
            }
            Underground = underground;
            MapInstance = map;
        }
    }
}