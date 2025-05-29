using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Slot4_Demo1.Models;

namespace Slot4_Demo1
{
    public static class ODataModelBuilderConfig
    {
        public static IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder modelBuilder = new ODataConventionModelBuilder();
            modelBuilder.EntitySet<Gadgets>("GadgetsOdata");
            return modelBuilder.GetEdmModel();
        }
    }
}