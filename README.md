# Demanda Green Roots: O Desafio das Emissões de CO2
Uma empresa tem uma meta clara de reduzir emissões, aquelas geradas em sua vasta cadeia de fornecedores de matéria-prima e componentes.

## Principais Dores da Empresa --
- Dados autodeclarados e não confiáveis;
- Falta de visibilidade operacional;
- Análise de causa raiz ineficaz.

## Solução --
Nossa solução é um passaporte digital chamado **Green Roots**, um ecossistema completo que cria um registro único, auditável e detalhado para os componentes críticos do caminhão, desde a origem até a linha de montagem.A solução Green Roots foi projetada para não ser um sistema isolado, mas sim um módulo estratégico que se 
integra perfeitamente ao ecossistema de gestão na empresa.

## Funcionalidade do Projeto --
Nossa estação, instalada diretamente na linha de produção do fornecedor, é o cérebro da captura de dados. 
Ela transforma a produção em inteligência, medindo o que realmente importa, e não apenas o que é reportado.

## Funcionalidades Implementadas --

### Autenticação ---
- Tela de login com e-mail e senha;
- Tela de cadastro de novo usuário;
- Validação de campos obrigatórios;
- Mensagens de erro para login inválido.

### Persistência de Dados ---
- Banco de dados local SQLite (Passaporte_Digital.db);
- Cadastro e autenticação de usuários persistidos entre sessões.

### Dashboard ---
- Tela inicial após login com nome do usuário;
- Botão de logout funcional;
- Preparado para integração com sensores Arduino (em desenvolvimento).

### Arquitetura ---
- Padrão MVVM (Model-View-ViewModel);
- Separação de responsabilidades entre View, ViewModel e Data.

## Criação de BrModelo --
### Modelo Conceitual ---
<img width="750" height="418" alt="Modelo_Conceitual" src="https://github.com/user-attachments/assets/db65e8a9-25df-46e2-bfff-d768fb57d9f5" />

### Modelo Lógico ---
<img width="750" height="418" alt="Modelo_Lógico" src="https://github.com/user-attachments/assets/7b06c73e-1921-4a59-9e0c-863f28d0bd62" />
