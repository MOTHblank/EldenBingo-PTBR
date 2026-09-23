**This fork aims to add a Brazilian Portuguese (PT-BR) localization to the project, while keeping the ability to alternate between EN and PT-BR.**

# Elden Bingo
Esta aplicação facilita a execução, administração, espectação e transmissão de corridas de Bingo no Elden Ring. Foi desenvolvida tendo o [Bingo Brawlers](https://bingobrawlers.com) em mente. É construída em .NET 8.0, portanto requer que os [runtimes](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-desktop-8.0.27-windows-x64-installer) estejam instalados.

![eldenbingo-window](https://user-images.githubusercontent.com/604653/236489862-7a69d672-9243-49fb-88fc-236ae502f655.png)  
![mapview](https://user-images.githubusercontent.com/604653/235779143-aa708a4e-0443-49fb-96b7-b8c3dce73e67.png)  
*Espectando dois jogadores*

# Visão Geral das Funcionalidades
* Hospedar um servidor capaz de executar várias corridas de bingo simultaneamente
* Criar lóbis de bingo com senha de administrador opcional e enviar seu próprio arquivo .json de bingo
* Entrar em lóbis como jogador individual, parte de uma equipe ou até mesmo como espectador
* Bate-papo nos lóbis
* Acompanhar a posição dos seus companheiros de equipe ao vivo no mapa integrado das Terras Intermédias (The Lands Between), ou ver todos como espectador
* Configurar seu lóbi com regras personalizadas, como classes iniciais aleatórias
* Suporte para partidas com bloqueio (lockout) e sem bloqueio
* As partidas podem ser iniciadas, pausadas e encerradas pelos árbitros
* Os quadrados podem ser marcados/desmarcados pelos jogadores ou pelos árbitros em nome dos jogadores
* Clique com o botão direito para marcar quadrados com estrelas, visíveis apenas para o jogador que marcou
* Rolar a roda do mouse para cima/para baixo sobre um quadrado permite que jogadores ou árbitros acompanhem seu progresso em um quadrado. Este progresso só pode ser visto pelo próprio jogador e espectadores, mas não pelos jogadores/equipes adversárias
* Personalizar cor/tamanho/fonte do tabuleiro de bingo
* Suporte a múltiplos idiomas (Inglês e Português do Brasil)

# Hospedando seu próprio servidor
Baixe o aplicativo normal, abra as Configurações (Settings) e ative "Hospedar servidor de bingo ao iniciar".
![host](https://user-images.githubusercontent.com/604653/235767838-ae5752a7-e9e7-4abb-a1d1-c8e6a59292aa.png)

Lembre-se de configurar o redirecionamento de portas (port forwarding) apropriado.

Você também pode hospedar um servidor de lóbi dedicado. Não disponibilizo mais binários de servidor nas versões regulares, mas se precisar deles, pode facilmente baixar o código e compilá-lo você mesmo.

# Conectando-se a um servidor e entrando em um lóbi
Conecte-se a um servidor clicando no botão 'Conectar' no canto superior esquerdo. Você pode optar por conectar-se automaticamente ao mesmo servidor toda vez que iniciar o aplicativo.

Depois de se conectar com sucesso ao servidor, você pode criar seu próprio lóbi ou entrar em um existente. Um 'Lóbi' neste aplicativo é uma sala privada onde você pode executar seu jogo de bingo. Qualquer jogador que deseje se conectar a um lóbi precisa do nome da sala desse lóbi. Não há navegador de lóbis.

## Entrando em um lóbi
Ao entrar (ou criar) um lóbi, você precisará inserir um apelido e selecionar uma equipe. No menu suspenso 'Equipe', você também pode selecionar 'Espectador'.
![join_spectator](https://user-images.githubusercontent.com/604653/235904929-2adf97ee-e6c4-4fc3-a8c7-586c383453d1.png)

## Criando um lóbi
Ao criar um lóbi, você pode inserir qualquer nome de sala que desejar ou usar o que foi gerado. Se inserir uma senha de admin, qualquer jogador que se conectar ao lóbi com essa mesma senha de admin também se tornará um administrador. Se deixar em branco, apenas você poderá administrar.

Você também pode configurar as regras do lóbi:
![image](https://github.com/user-attachments/assets/44d037ec-13d9-4220-bcf1-7a4d53c3fc60)

* *Tamanho do tabuleiro* especifica o quão grande o tabuleiro de bingo será. Isso se aplica na próxima vez que um tabuleiro for gerado.
* *Lockout (Bloqueio)* especifica que apenas um jogador/equipe pode marcar o mesmo quadrado. Se desmarcado, vários jogadores/equipes poderão marcar o mesmo quadrado.
* *Semente aleatória (Random seed)* garantirá que a mesma sequência de tabuleiros e classes aleatórias seja gerada/escolhida. Esta sequência será redefinida quando um novo json for enviado. **0 significa que uma semente aleatória será usada**.
* *Tempo de Preparação* cria uma fase de preparação extra no início da partida, após a contagem regressiva inicial, na qual os jogadores podem ver o tabuleiro e as classes disponíveis e se planejar antes do início da partida. **0 significa sem fase de preparação**.
* *Pontos bônus por bingo* pode ser usado se você quiser que as linhas de bingo valham um número definido de pontos em vez de encerrar imediatamente a partida.
* *Limitar classes iniciais* pode ser usado se você quiser limitar a escolha de classes iniciais para introduzir alguma variação. Defina o conjunto de classes possíveis abaixo.
* A opção *Máx. de quadrados na mesma categoria* garantirá que no máximo esse número de quadrados da mesma categoria seja incluído em um tabuleiro. **0 significa que este recurso está desativado**. Para mais informações sobre categorias e o formato json, consulte [Formato Json](#formato-json).

# Administrando um lóbi
Você não obtém nenhuma vantagem injusta no jogo como administrador, então pode participar da partida normalmente. Apenas AdminEspectadores (ou seja, um jogador que é administrador e espectador ao mesmo tempo) têm privilégios especiais (consulte [AdminEspectadores](#adminespectadores)).

Quando você se juntar a um lóbi como administrador, as ferramentas de administração aparecerão abaixo do tabuleiro de bingo. Use estas ferramentas para enviar um arquivo Bingo .json, seguindo o mesmo formato do Bingo Brawlers e BingoSync, mas com algumas extensões. [Aqui está um arquivo de exemplo](https://bingobrawlers.com/files/bingo-brawlers.json). Para mais informações sobre o formato json, consulte [Formato Json](#formato-json).

Assim que você enviar o arquivo, um tabuleiro é gerado, mas não ficará visível para os jogadores até que a partida seja iniciada. AdminEspectadores podem ver o tabuleiro e gerar novos tabuleiros se necessário.

Use os botões de controle de partida na parte inferior para iniciar, pausar ou parar a partida.
![admin-controls](https://user-images.githubusercontent.com/604653/235774234-1d690243-9827-4510-9e51-a0befd3f0b78.png)  

# Visualização de Mapa (Map View)
Clique no botão 'Abrir Mapa' na parte superior para exibir a janela do Mapa. Esta janela OpenGL mostrará um mapa das Terras Intermédias e a posição de todos os jogadores da sua equipe. Como espectador, você poderá ver todos os jogadores simultaneamente. O mapa tentará enquadrar todos os jogadores visíveis ao mesmo tempo.

## Aparecendo no mapa
O aplicativo tentará detectar uma instância em execução do EldenRing.exe. Você também pode clicar no botão 'Iniciar Elden Ring' no canto superior direito para que o aplicativo inicie o Elden Ring sem o Easy Anti-Cheat ativado. Isso é necessário para que o aplicativo possa ler a memória do jogo e buscar sua posição atual. Ele sempre mostrará o mapa da superfície, mesmo se os jogadores estiverem no subsolo. Jogadores no subsolo serão renderizados levemente transparentes.

## Desenhando no mapa
Você pode usar o botão direito do mouse para desenhar no mapa. Isso se destina a transmissões ao vivo e é completamente no lado do cliente no momento.
![telestrator](https://github.com/awsker/EldenBingo/assets/604653/98aa472b-fffd-48d4-9420-aba1b8df25b4)

## Controles do Mapa
* Clique Esquerdo - Mover o mapa (para de seguir jogadores)
* Clique Direito - Desenhar no mapa
* Roda do mouse - Dar zoom in e zoom out
* N - Alternar visibilidade das etiquetas de nome
* Z - Desfazer a última linha desenhada
* C - Limpar todas as linhas
* F - Ajustar todos os jogadores na tela
* X - Alternar entre camadas (Jogo principal e DLC)
* 1-9 - Seguir um jogador específico

## Exibição de escolha de classe
Se você ativar a configuração "Mostrar classes disponíveis em uma camada no mapa", as classes iniciais disponíveis serão exibidas na Janela do Mapa quando a partida começar. Isso é útil se você for um streamer e quiser que seus espectadores vejam a seleção. Basta configurar uma cena no seu software de transmissão que capture a janela do mapa (como captura de jogo) e pronto. Clicar com o botão esquerdo do mouse, pressionar Espaço ou Esc fechará a exibição das classes.
![random classes](https://github.com/awsker/EldenBingo/assets/604653/562f384c-231e-42fd-8234-9715887b377d)


# Controles do tabuleiro de bingo
* Clique esquerdo - Marcar ou desmarcar um quadrado para você/sua equipe. Visível para todos.
* Clique direito - Marcar um quadrado com uma estrela. A estrela pode ser usada para qualquer coisa, como um lembrete para você mesmo. A estrela é visível apenas para você.
* Rolar roda do mouse para cima/baixo - Aumentar/diminuir a contagem deste quadrado. O contador é útil para quadrados que têm um número definido de tarefas a serem concluídas, onde é fácil perder o controle do progresso. O contador é visível apenas para sua própria equipe e espectadores.

# AdminEspectadores
Como AdminEspectador, você é basicamente o árbitro da partida. Você pode ver o tabuleiro de bingo gerado antes do início do jogo e gerar novos tabuleiros. Se selecionar um jogador na lista de clientes, poderá realizar ações no tabuleiro em nome desse jogador, como marcar/desmarcar quadrados e incrementar/decrementar a contagem de um quadrado.
![counters](https://user-images.githubusercontent.com/604653/235781324-d6e7f488-9c25-4920-b6be-682e061e8987.png)  

# Configurações
As configurações são voltadas principalmente para a conveniência de um streamer, para ajustar os componentes da interface ao tamanho e posição corretos para serem facilmente capturados no software de transmissão. Você também pode ativar a hospedagem de servidor a partir daqui.

# Formato Json
O formato é o mesmo usado pelo Bingo Brawlers e BingoSync, mas com extensões para categorias, quadrado central e substituição de tokens.
<img width="1139" height="99" src="https://github.com/user-attachments/assets/ac02dbce-a7de-4c22-aa4b-5d0a98e8c8ec" />


Use a chave **category** para definir uma única categoria, ou a chave **categories** para definir uma matriz de categorias. Essas categorias podem ser usadas em conjunto com a configuração do lóbi *Max square in same category* para garantir que no máximo esse número de categorias esteja presente em um tabuleiro de bingo, a fim de gerar tabuleiros de bingo mais equilibrados.

Se um ou mais quadrados tiverem a tag **center** definida como 1, um deles será selecionado aleatoriamente para ser o quadrado central em tamanhos de tabuleiro onde isso for aplicável.

Você pode marcar um quadrado com **color** e especificar uma cor como valor, seja como [Nome de Cor](https://htmlcolorcodes.com/color-names/) ou Código de Cor HTML. O texto naquele quadrado será exibido com essa cor se o jogador tiver ativado a exibição dessas cores sugeridas.

Tokens podem ser usados para criar quadrados mais dinâmicos. Crie um token envolvendo uma palavra com sinais de porcentagem (por exemplo %x%) e declare uma lista de possíveis substituições em uma matriz com o mesmo nome do token. Uma delas será escolhida aleatoriamente quando o quadrado for gerado. Você pode até ter múltiplos tokens no mesmo quadrado. Veja o exemplo na imagem acima.

# Como criar uma Release
Para gerar e publicar uma nova release do projeto **EldenBingo-PTBR**:

1. **Atualizar a Versão no Código somente quando o código funcional mudar:**
   - Verifique e atualize a versão em `EldenBingoCommon/Version.cs` (ex.: `CurrentVersion => "0.19.0.1"`).
2. **Compilar os Binários de Release:**
   - Execute o comando de compilação/publicação para a plataforma alvo (Windows x64):
     ```bash
     dotnet publish EldenBingo/EldenBingo.csproj -c Release -r win-x64 -p:EnableWindowsTargeting=true --self-contained false -o ./release
     ```
3. **Empacotar os Arquivos do Release:**
   - Comprima os arquivos gerados no diretório `./release` para um arquivo `.zip` (ex.: `EldenBingo_v0.19.0.1.zip`).
4. **Criar a Tag no Git:**
   - Crie uma tag anotada para a versão correspondente e faça o push para o repositório no GitHub:
     ```bash
     git tag 0.19.0.1
     git push origin 0.19.0.1
     ```
5. **Publicar no GitHub Releases:**
   - Acesse as releases do repositório `MOTHblank/EldenBingo-PTBR` no GitHub.
   - Crie uma nova Release selecionando a tag `0.19.0.1`.
   - Adicione o título no padrão upstream (ex.: `Elden Bingo v0.19.0.1`) e as notas da versão.
   - Anexe o arquivo `.zip` empacotado como asset da release e publique.
   - Mudanças somente no empacotamento não exigem nova versão; substitua o asset da mesma release.

# Créditos
* Nordgaren por injeção de assembly em processos
* Tremwil no Discord de The Grand Archives
* Código de leitura de processo extraído de [EldenRingFPSUnlockerAndMore](https://github.com/uberhalit/EldenRingFpsUnlockAndMore) por [uberhalit](https://github.com/uberhalit)
* Imagens dos botões gentilmente cedidas por [EldenRingMap](https://eldenringmap.com), desenhadas por [Caio Razera](https://dcaier.artstation.com/)
