﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Microservice.Models;

public partial class TokenTable
{
    public int IdToken { get; set; }

    public int IdUser { get; set; }

    public string AccessToken { get; set; }

    public string RefreshToken { get; set; }

    public DateTime? CreateAt { get; set; }

    public virtual User IdUserNavigation { get; set; }
}