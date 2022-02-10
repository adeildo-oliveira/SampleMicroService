using System.ComponentModel;

namespace CommonLib.Models;

public enum TipoStatus
{
    Sim = 1,
    Nao = 0
}

public enum UF
{
    [Description("Distrito Federal")]
    DF = 0,
    [Description("Acre")]
    AC = 1,
    [Description("Alagoas")]
    AL = 2,
    [Description("Amapá")]
    AP = 3,
    [Description("Amazonas")]
    AM = 4,
    [Description("Bahia")]
    BA = 5,
    [Description("Ceará")]
    CE = 6,
    [Description("Espírito Santo")]
    ES = 7,
    [Description("Goiás")]
    GO = 8,
    [Description("Maranhão")]
    MA = 9,
    [Description("Mato Grosso")]
    MT = 10,
    [Description("Mato Grosso do Sul")]
    MS = 11,
    [Description("Minas Gerais")]
    MG = 12,
    [Description("Pará")]
    PA = 13,
    [Description("Paraíba")]
    PB = 14,
    [Description("Paraná")]
    PR = 15,
    [Description("Pernambuco")]
    PE = 16,
    [Description("Piauí")]
    PI = 17,
    [Description("Rio de Janeiro")]
    RJ = 18,
    [Description("Rio Grande do Norte")]
    RN = 19,
    [Description("Rio Grande do Sul")]
    RS = 20,
    [Description("Rondônia")]
    RO = 21,
    [Description("Roraima")]
    RR = 22,
    [Description("Santa Catarina")]
    SC = 23,
    [Description("São Paulo")]
    SP = 24,
    [Description("Sergipe")]
    SE = 25,
    [Description("Tocantins")]
    TO = 26
}