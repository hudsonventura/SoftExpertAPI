# AlterUserStart

Altera o usuário iniciador (requester) de uma instância em andamento via SOAP `editWorkflowData`.

> Instanciação: [Instanciacao.md](../Instanciacao.md)

## Parâmetros de entrada

| Nome | Tipo | Obrigatório | Descrição |
| --- | --- | --- | --- |
| `workflowID` | `string` | Sim | ID da instância |
| `requesterID` | `string` | Sim | Matrícula do novo iniciador |
| `explanation` | `string` | Não | Mantido por compatibilidade; não é enviado no SOAP |

A instância precisa estar com status `Em_Andamento`; caso contrário, lança `SoftExpertException`.

## Retorno

Não retorna valor.

## Exemplo

```csharp
wfAPI.AlterUserStart(
    workflowID: "CCF202614358",
    requesterID: "sistema.teste",
    explanation: "Alteração do iniciador via SoftExpertAPI"
);
```
