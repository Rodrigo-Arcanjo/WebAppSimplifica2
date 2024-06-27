# WebAppSimplifica2

Este projeto foi criado como uma nova API para a versão mobile do site Simplifica Salvador utilizando ASP.NET Core 7. O principal objetivo deste desenvolvimento foi elaborar um protótipo de Web Service que atendesse a todas as requisições necessárias para o funcionamento do aplicativo móvel. A arquitetura do projeto, bem como as configurações necessárias para o uso de bibliotecas e outras ferramentas, foram todas concebidas com foco em desempenho e organização, aplicando metodologias que otimizassem esses aspectos.

A seguir, destaco três pontos principais deste projeto:

Documentação via Swagger: Todas as controladoras e seus métodos foram documentados utilizando o Swagger, proporcionando uma interface interativa e compreensível para a exploração e teste das APIs disponíveis. Isso facilita o entendimento e o uso das funcionalidades implementadas por desenvolvedores e integradores.

Separação de Arquivos 'appsettings': Foi implementada a separação dos arquivos de configuração appsettings para diferentes ambientes, como produção e homologação. Essa abordagem garante que cada ambiente possua suas próprias configurações específicas, aumentando a segurança e a eficiência na gestão das variáveis de configuração.

Criação e Validação via Token OAuth2: Para a autenticação e autorização das requisições, foi implementado um sistema de criação e validação de tokens utilizando o padrão OAuth2. Essa escolha assegura um alto nível de segurança, permitindo que apenas usuários autenticados e autorizados acessem os recursos protegidos da API.

Este projeto representa um esforço significativo na criação de uma infraestrutura de serviço web robusta e escalável, atendendo às necessidades do aplicativo móvel de forma eficiente e segura.
