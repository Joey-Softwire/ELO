﻿using System.Data.Entity;
using EloWeb.Models;

namespace EloWeb
{
    public class PoolLadderDbInitializer: DropCreateDatabaseAlways<PoolLadderContext> { }
}