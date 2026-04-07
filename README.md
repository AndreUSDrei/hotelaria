# Plaza Hotels - Sistema de Hotelaria

Sistema de gerenciamento de reservas de hotel desenvolvido em ASP.NET Core MVC com implementação de padrões de projeto.

## Padrões de Projeto Implementados

### 1. Singleton - GerenciadorReservas
Garante uma única instância para controle centralizado de reservas, disponibilidade, check-in e check-out.

```csharp
// Acesso global à instância única
var gerenciador = GerenciadorReservas.Instancia;
```

### 2. Abstract Factory - IPacoteHospedagemFactory
Cria famílias de objetos relacionados (quarto, café da manhã, serviço) sem especificar classes concretas.

```csharp
// Obter fábrica de pacote específico
var factory = PacoteFactoryProvider.ObterFactory("Romantico");

// Criar produtos da família
var quarto = factory.CriarQuarto();      // QuartoLuxo
var cafe = factory.CriarCafeDaManha();   // CafeRomantico
var servico = factory.CriarServico();    // JantarRomantico
```

### 3. Factory Method - QuartoFactory
Define interface para criação de objetos, permitindo extensibilidade para novos tipos.

```csharp
// Criar quarto por tipo
var quarto = QuartoFactory.CriarQuarto("Luxo");
```

## Estrutura do Projeto

```
PlazaHotels/
|-- Controllers/
|   |-- HomeController.cs      # Páginas públicas
|   |-- AdminController.cs     # Painel administrativo
|   |-- ApiController.cs       # API REST
|
|-- Models/
|   |-- Interfaces/            # Contratos
|   |-- Quartos/               # Produtos concretos
|   |-- Refeicoes/             # Produtos concretos
|   |-- Servicos/              # Produtos concretos
|   |-- Factories/             # Fábricas
|   |-- Services/              # Singleton
|   |-- Entities/              # Entidades
|   |-- ViewModels/            # Models para Views
|
|-- Views/
|   |-- Home/                  # Views públicas
|   |-- Admin/                 # Views administrativas
|
|-- DIAGRAMA_CLASSES.md        # Diagrama Mermaid completo
```

## Funcionalidades

- **Catálogo de Quartos**: Standard, Luxo e Suíte Presidencial
- **Pacotes de Hospedagem**: Standard, Negócios, Romântico e Premium
- **Sistema de Reservas**: Criação, confirmação e cancelamento
- **Check-in/Check-out**: Gestão de hóspedes
- **Verificação de Disponibilidade**: Consulta em tempo real
- **Painel Administrativo**: Dashboard com estatísticas
- **API REST**: Integração com sistemas externos

## Como Executar

```bash
# Entrar na pasta do projeto
cd c:\Users\Pedro\Desktop\Hotelaria

# Executar a aplicação
dotnet run

# Acessar no navegador
# https://localhost:5001 ou http://localhost:5000
```

## Endpoints da API

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | /api/api/quartos | Lista todos os quartos |
| GET | /api/api/pacotes | Lista todos os pacotes |
| GET | /api/api/disponibilidade | Verifica disponibilidade |
| GET | /api/api/reservas | Lista reservas ativas |
| POST | /api/api/reservar | Cria nova reserva |
| POST | /api/api/checkin/{id} | Realiza check-in |
| POST | /api/api/checkout/{id} | Realiza check-out |

## Fluxo de Execução

```
1. Hóspede escolhe datas
   |
   v
2. GerenciadorReservas (Singleton) verifica disponibilidade
   |
   v
3. Hóspede seleciona pacote
   |
   v
4. PacoteFactoryProvider fornece a fábrica correta
   |
   v
5. IPacoteHospedagemFactory (Abstract Factory) monta o pacote
   |
   v
6. Reserva registrada no Singleton
   |
   v
7. Confirmação exibida ao hóspede
```

## Tecnologias

- **Framework**: ASP.NET Core 9.0 MVC
- **Linguagem**: C# 13
- **Frontend**: Razor Views + Bootstrap 5
- **Ícones**: Bootstrap Icons

## Diagrama de Classes

Ver arquivo `DIAGRAMA_CLASSES.md` para o diagrama completo em Mermaid.

## Autor

Projeto desenvolvido para disciplina de Padrões de Projeto.

---

**Plaza Hotels** - Sistema de Hotelaria com Padrões de Projeto
