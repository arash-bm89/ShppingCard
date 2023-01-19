﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Models;

namespace Common.ExtensionMethods
{
    public static class ModelBaseExtensions
    {
        public static void SetBaseModelPropsToRequest<TDestination>(
            this ModelBase source,
            TDestination destination)
            where TDestination: ModelBase
        {
            destination.Id = source.Id;
            destination.SeqId = source.SeqId;
            destination.Version = source.Version;
            destination.CreatedAt = source.CreatedAt;
        }

    }
}
