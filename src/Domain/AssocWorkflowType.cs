namespace SoftExpertAPI;

/// <summary>
/// Tipo de associação entre instâncias de workflow (newAssocWorkflow).
/// </summary>
public enum AssocWorkflowType
{
    /// <summary>Associação não bloqueante (padrão).</summary>
    NaoBloqueante = 0,

    /// <summary>Associação bloqueante.</summary>
    Bloqueante = 1,
}