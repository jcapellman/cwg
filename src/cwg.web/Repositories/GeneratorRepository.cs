﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using cwg.web.Generators;

namespace cwg.web.Repositories
{
    public class GeneratorRepository
    {
        private static List<T> GetObjects<T>()
        {
            var types = Assembly.GetExecutingAssembly().GetTypes().Where(a => a.BaseType == typeof(T) && !a.IsAbstract);

            return types.Select(b => (T)Activator.CreateInstance(b)).ToList();
        }

        public static IEnumerable<BaseGenerator> GetGenerators()
        {
            var baseGenerators = GetObjects<BaseGenerator>().Where(a => a.Active).ToList();

            baseGenerators.AddRange(GetObjects<BaseArchiveGenerator>().Where(a => a.Active));

            return baseGenerators.OrderBy(a => a.Name);
        }
    }
}