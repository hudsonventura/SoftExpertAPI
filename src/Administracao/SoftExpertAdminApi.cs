﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace SoftExpert.Admin;

public class SoftExpertAdminApi : SoftExpertBaseAPI
{
    public SoftExpertAdminApi(string baseUrl, Dictionary<string, string> headers, IDataBase db = null) : base(baseUrl, headers, db)
    {
    }

    public SoftExpertAdminApi(string baseUrl, string authorization, IDataBase db = null) : base(baseUrl, authorization, db)
    { 
    }

    protected override void SetUriModule()
    {
        _uriModule = "/apigateway/se/ws/adm_ws.php";
    }




    /// <summary>
    /// Habilita um usuário no SoftExpert
    /// </summary>
    /// <param name="UserID">Matrícula do usuário</param>
    /// <exception cref="Exception"></exception>
    public void enableUser(string UserID){
        ValidateDB();
        
        string sql = $"update {db_name}.aduser set fguserenabled = 1 where iduser = :UserID";

        Dictionary<string, dynamic> parametros = new Dictionary<string, dynamic>();
        parametros.Add(":UserID", UserID);

        try
        {
            db.Execute(sql, parametros);
            return;
        }
        catch (Exception erro)
        {
            throw new Exception($"Falha executar SQL no banco de dados. Erro: {erro.Message}");
        }
        
    }

    /// <summary>
    /// Desabilita um usuário no SoftExpert
    /// </summary>
    /// <param name="UserID">Matrícula do usuário</param>
    /// <exception cref="Exception"></exception>
    public void disableUser(string UserID){
        ValidateDB();
        
        string sql = $"update {db_name}.aduser set fguserenabled = 0 where iduser = :UserID";

        Dictionary<string, dynamic> parametros = new Dictionary<string, dynamic>();
        parametros.Add(":UserID", UserID);

        try
        {
            db.Execute(sql, parametros);
            return;
        }
        catch (Exception erro)
        {
            throw new Exception($"Falha executar SQL no banco de dados. Erro: {erro.Message}");
        }
    }

    
}

