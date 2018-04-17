using System;
using System.Collections.Generic;
using System.Linq;
using ArmLancer.Core.Interfaces;
using ArmLancer.Data.Context;
using ArmLancer.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ArmLancer.Core.Impl
{
    public class CategoryService : CrudService<Category>, ICategoryService
    {
        public CategoryService(
            IServiceProvider serviceProvider,
            ArmLancerDbContext context) : base(serviceProvider, context)
        {
        }

        public IEnumerable<Category> GetAllRecursive(long? parentId = null)
        {
            return _context.Categories.Where(c => c.ParentId == parentId)
                .Include(c => c.Children);
        }

        public IEnumerable<Category> GetList(long? parentId)
        {
            return _context.Categories.Where(c => c.ParentId == parentId);
        }
    }
}