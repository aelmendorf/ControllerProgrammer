using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerProgrammer.Data.Model {
    public class ProgrammingContextFactory : IDesignTimeDbContextFactory<ProgrammerContext> {
        public ProgrammerContext CreateDbContext(string[] args) {
            var optionsBuilder = new DbContextOptionsBuilder<ProgrammerContext>();
            optionsBuilder.UseSqlite("Data Source=ControllerData.db");
            return new ProgrammerContext(optionsBuilder.Options);
        }
    }
}
