using System.Globalization;

namespace EldenBingoCommon
{
    public static class LocalizationManager
    {
        public static event Action? LanguageChanged;

        private static string _currentLanguage = "en";

        public static string CurrentLanguage
        {
            get => _currentLanguage;
            set
            {
                var newLang = value?.ToLowerInvariant() == "pt-br" || value?.ToLowerInvariant() == "pt" ? "pt-BR" : "en";
                if (_currentLanguage != newLang)
                {
                    _currentLanguage = newLang;
                    LanguageChanged?.Invoke();
                }
            }
        }

        public static bool IsPtBr => CurrentLanguage == "pt-BR";

        private static readonly Dictionary<string, string> PtBrStrings = new(StringComparer.OrdinalIgnoreCase)
        {
            // Main Window / Menu / Toolstrip
            { "Connect", "Conectar" },
            { "Disconnect", "Desconectar" },
            { "Create Lobby", "Criar Lóbi" },
            { "Join Lobby", "Entrar no Lóbi" },
            { "Leave Lobby", "Sair do Lóbi" },
            { "Change Team", "Mudar de Equipe" },
            { "Open Map", "Abrir Mapa" },
            { "Pop-Out Board", "Destacar Tabuleiro" },
            { "Settings", "Configurações" },
            { "Start Elden Ring", "Iniciar Elden Ring" },
            { "Console", "Console" },
            { "Lobby", "Lóbi" },

            // Connect Form
            { "Connect To Bingo Server", "Conectar ao Servidor de Bingo" },
            { "Address:", "Endereço:" },
            { "Port:", "Porta:" },
            { "Cancel", "Cancelar" },
            { "Auto-connect:", "Conectar auto:" },

            // Create / Join Lobby Form
            { "Room name:", "Nome da sala:" },
            { "Admin password (optional):", "Senha de admin (opcional):" },
            { "Nickname:", "Apelido:" },
            { "Team:", "Equipe:" },
            { "Lobby Settings <<", "Configurações do Lóbi <<" },
            { "Lobby Settings >>", "Configurações do Lóbi >>" },
            { "Seed: ", "Semente: " },

            // Change Team Form
            { "OK", "OK" },

            // Admin Controls
            { "Upload Bingo JSON:", "Enviar JSON de Bingo:" },
            { "Browse", "Procurar" },
            { "Upload", "Enviar" },
            { "Randomize New Board", "Gerar Novo Tabuleiro" },
            { "Start Match", "Iniciar Partida" },
            { "Pause Match", "Pausar Partida" },
            { "Unpause Match", "Retomar Partida" },
            { "Stop Match", "Parar Partida" },
            { "Admin Controls", "Controles de Admin" },
            { "Edit Lobby Settings", "Editar Configs do Lóbi" },
            { "AdminSpectator Info: Check/count actions are made on behalf of selection's team", "Info do AdminEspectador: Ações de marcar/contar são feitas em nome da equipe selecionada" },

            // Game Settings
            { "Board size:", "Tamanho do tabuleiro:" },
            { "Lockout", "Bloqueio (Lockout)" },
            { "Random seed:", "Semente aleatória:" },
            { "Limit starting classes:", "Limitar classes iniciais:" },
            { "Max squares in same category:", "Máx. de quadrados na mesma categoria:" },
            { "Preparation time:", "Tempo de preparação:" },
            { "seconds", "segundos" },
            { "Reset", "Redefinir" },
            { "Bonus points for bingo:", "Pontos bônus por bingo:" },
            { "Lobby Settings", "Configurações do Lóbi" },

            // Lobby Control
            { "Download Match Log", "Baixar Registro da Partida" },
            { "Send a message", "Enviar uma mensagem" },
            { "(as Json)", "(como Json)" },

            // Settings Dialog
            { "General", "Geral" },
            { "Bingo Board", "Tabuleiro de Bingo" },
            { "Map", "Mapa" },
            { "Hotkeys", "Atalhos" },
            { "Appearance", "Aparência" },
            { "Window Background Color:", "Cor de Fundo da Janela:" },
            { "Always on Top", "Sempre no Topo" },
            { "Server Hosting", "Hospedagem de Servidor" },
            { "Host a bingo server on launch", "Hospedar servidor de bingo ao iniciar" },
            { "Application Updates", "Atualizações do Aplicativo" },
            { "Check for updates on startup", "Verificar atualizações ao iniciar" },
            { "Sounds", "Sons" },
            { "Enable alert sounds", "Ativar sons de alerta" },
            { "Volume", "Volume" },
            { "Special alert when currently hovered square is marked by opponent (sniped)", "Alerta especial quando o quadrado sob o cursor for marcado pelo oponente (sniped)" },
            { "Output Device:", "Dispositivo de Saída:" },
            { "Play Test Sfx", "Testar Som" },

            { "Bingo Board Max Size", "Tamanho Máx. do Tabuleiro" },
            { "No Maximum Size", "Sem Tamanho Máximo" },
            { "Custom Max Size", "Tamanho Máx. Personalizado" },
            { "Font and Size:", "Fonte e Tamanho:" },
            { "Square Shadow Opacity", "Opacidade da Sombra dos Quadrados" },
            { "Highlight Marked Squares", "Destacar Quadrados Marcados" },
            { "Highlight Bingo Lines", "Destacar Linhas de Bingo" },
            { "Keyword Colors", "Cores por Palavra-chave" },
            { "Edit...", "Editar..." },
            { "Apply colors suggested by board", "Aplicar cores sugeridas pelo tabuleiro" },
            { "Text Color Intensity", "Intensidade da Cor do Texto" },
            { "Spectator Settings", "Configurações de Espectador" },
            { "When spectating, delay all match events (this includes square checks, counters, match status changes, timer etc.):", "Ao espectar, atrasar todos os eventos da partida (marcações, contadores, status, cronômetro, etc.):" },
            { "milliseconds", "milissegundos" },

            { "Map Initial Position", "Posição Inicial do Mapa" },
            { "Relative to Window", "Relativo à Janela" },
            { "Custom Position", "Posição Personalizada" },
            { "Map Initial Size", "Tamanho Inicial do Mapa" },
            { "Remember Last Size", "Lembrar Último Tamanho" },
            { "Custom Size", "Tamanho Personalizado" },
            { "Misc.", "Diversos" },
            { "Swap mouse buttons***(Left = Draw, Right = Pan)", "Inverter botões do mouse***(Esquerdo = Desenhar, Direito = Mover)" },
            { "Show available classes in an overlay on the map (for streaming)", "Mostrar classes disponíveis em uma camada no mapa (para transmissão)" },
            { "Map Framerate Limit (0 to disable)", "Limite de FPS do Mapa (0 para desativar)" },

            { "Bingo Board Hotkeys", "Atalhos do Tabuleiro de Bingo" },
            { "↑ Up", "↑ Cima" },
            { "↓ Down", "↓ Baixo" },
            { "← Left", "← Esquerda" },
            { "→ Right", "→ Direita" },
            { "↖️ Up Left", "↖️ Cima Esquerda" },
            { "↗️ Up Right", "↗️ Cima Direita" },
            { "↙️ Down Left", "↙️ Baixo Esquerda" },
            { "↘️ Down Right", "↘️ Baixo Direita" },
            { "✔️ Mark Square", "✔️ Marcar Quadrado" },
            { "⭐ Star Square", "⭐ Estrelar Quadrado" },
            { "➕ Increment Count", "➕ Incrementar Contador" },
            { "➖ Decrement Count", "➖ Decrementar Contador" },
            { "Enable hotkeys only if these modifier keys are held:", "Ativar atalhos apenas se estas teclas modificadoras estiverem pressionadas:" },
            { "Shift", "Shift" },
            { "Control", "Control" },
            { "Left Alt", "Alt Esquerdo" },
            { "Note: Mouse wheel is always bound to increment and decrement the counter for the hovered square", "Nota: A roda do mouse sempre incrementa e decrementa el contador do quadrado sob o cursor" },

            // Keyword Colors Editor Form
            { "Keyword Colors Editor", "Editor de Cores por Palavra-chave" },
            { "Help", "Ajuda" },
            { "Keyword", "Palavra-chave" },
            { "Color", "Cor" },
            { "The first rule (top to bottom) with a keyword found in a square's text (case-insensitive) will be used to color that square's text", "A primeira regra (de cima para baixo) com uma palavra-chave encontrada no texto do quadrado (sem diferenciar maiúsculas) será usada para colorir o texto" },
            { "New Ruleset...", "Novo Conjunto de Regras..." },
            { "Open Ruleset...", "Abrir Conjunto de Regras..." },
            { "Save Ruleset To File...", "Salvar Conjunto de Regras em Arquivo..." },

            // Map Window & Popout Board Form
            { "Bingo Board", "Tabuleiro de Bingo" },
            { "Map Window", "Janela do Mapa" },
            { "Textures loading...", "Carregando texturas..." },

            // Elden Ring Starting Classes (Official BR Translation)
            { "Vagabond", "Vagabundo" },
            { "Warrior", "Guerreiro" },
            { "Hero", "Herói" },
            { "Bandit", "Bandido" },
            { "Astrologer", "Astrólogo" },
            { "Prophet", "Profeta" },
            { "Samurai", "Samurai" },
            { "Prisoner", "Prisioneiro" },
            { "Confessor", "Confessor" },
            { "Wretch", "Miserável" },
            { "IdusKnight", "Cavaleiro Idus" },
            { "HeavyKnight", "Cavaleiro Pesado" },

            // Teams & Colors
            { "Spectator", "Espectador" },
            { "Red Team", "Equipe Vermelha" },
            { "Blue Team", "Equipe Azul" },
            { "Green Team", "Equipe Verde" },
            { "Orange Team", "Equipe Laranja" },
            { "Purple Team", "Equipe Roxa" },
            { "Cyan Team", "Equipe Ciano" },
            { "Pink Team", "Equipe Rosa" },
            { "Brown Team", "Equipe Marrom" },
            { "Yellow Team", "Equipe Amarela" },

            // Messages & Dialogs
            { "Disconnect from server?", "Desconectar do servidor?" },
            { "Leave current lobby?", "Sair do lóbi atual?" },
            { "Stop match? The match will end immediately", "Parar partida? A partida será encerrada imediatamente" },
            { "Stop match", "Parar partida" },
            { "Language", "Idioma" },
            { "English", "Inglês (English)" },
            { "Portuguese (Brazil)", "Português (Brasil)" },
            { "Change Team Name", "Mudar Nome da Equipe" }
        };

        public static string GetString(string key)
        {
            if (string.IsNullOrEmpty(key)) return key;

            if (IsPtBr && PtBrStrings.TryGetValue(key, out var translated))
            {
                return translated;
            }

            return key;
        }

        public static string GetEldenRingClassName(EldenRingClasses eldenClass)
        {
            var name = eldenClass.ToString();
            return GetString(name);
        }

        public static string GetTeamName(int team)
        {
            if (team == -1)
                return GetString("Spectator");

            string[] teamKeys = new[]
            {
                "Red Team", "Blue Team", "Green Team", "Orange Team",
                "Purple Team", "Cyan Team", "Pink Team", "Brown Team", "Yellow Team"
            };

            if (team >= 0 && team < teamKeys.Length)
            {
                return GetString(teamKeys[team]);
            }

            return string.Empty;
        }
    }
}
