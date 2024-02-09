using System;
using System.Collections.Generic;
using System.Net.Http;
using SoftExpert;
using SoftExpertAPI.Interfaces;

namespace src;

public abstract class SoftExpertBaseAPI
{
    protected string _uriModule = null;
    protected readonly HttpClient restClient;

    protected IDataBase db = null;

    public string db_name = null;

    /// <summary>
    /// Construtor. Necessário passar a URL completa do ambiente do SE e os headers. Header Authorization é necessário
    /// </summary>
    /// <param name="url">URL completa do ambiente. Ex.: https://se.example.com.br</param>
    /// <param name="headers">Passar os headers a serem enviados na requisição. Não esqueça do header Authorization</param>
    /// <param name="db">Classe concreta que implemente a inteface SoftExpertAPI.Interfaces.IDataBase</param>
    public SoftExpertBaseAPI(string baseUrl, Dictionary<string, string> headers, SoftExpertAPI.Interfaces.IDataBase db = null)
    {
        restClient = new HttpClient();
        restClient.BaseAddress = new Uri(baseUrl);
        foreach (var head in headers)
        {
            restClient.DefaultRequestHeaders.Add(head.Key, head.Value);
        }
        //restClient.AddDefaultHeader("Host", url.Split("://")[1].Split(":")[0]);
        this.db = db;
        this.db_name = db.db_name;
        SetUriModule();
    }

    /// <summary>
    /// Construtor. Necessário passar a URL completa do ambiente do SE e a string to Authorization incluindo o 'Basic ....'
    /// </summary>
    /// <param name="url">URL completa do ambiente. Ex.: https://se.example.com.br</param>
    /// <param name="authorization">Basic no formato base64("dominio\usuario:senha") Ex.: Basic dmMgPyB1bSBjdXJpb3Nv</param>
    public SoftExpertBaseAPI(string baseUrl, string authorization, SoftExpertAPI.Interfaces.IDataBase db = null)
    {
        restClient = new HttpClient();
        restClient.BaseAddress = new Uri(baseUrl);
        restClient.DefaultRequestHeaders.Add("Authorization", authorization);
        //restClient.AddDefaultHeader("Host", url.Split("://")[1].Split(":")[0]);
        this.db = db;
        this.db_name = db.db_name;
        SetUriModule();
    }

    protected virtual void SetUriModule()
    {
        throw new NotImplementedException("Atribuir a propriedade _uriModule");
    }

    


    protected void ValidateDB(){
        if (db is null) {
            throw new SoftExpertException("Uma instancia de banco de dados não foi informada na criação deste objeto. Crie forneça uma conexão com seu banco de dados implementando a interface SoftExpertAPI.Interfaces.IDataBase");
        }

        if (db_name is null) {
            throw new SoftExpertException("Uma instancia de banco de dados foi informada mas o nome do seu banco precisa ser informado na propriedade db_name");
        }
    }

}

