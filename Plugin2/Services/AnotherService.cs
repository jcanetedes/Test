﻿using Test2.Core.Interfaces;

namespace Plugin2.Services
{
    public class AnotherService : IAnotherService,IService
    {
        public void Check()
        {
            throw new NotImplementedException();
        }
    }
}
