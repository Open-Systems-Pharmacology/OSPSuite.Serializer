using System.Linq;
using OSPSuite.BDDHelper.Extensions;

namespace OSPSuite.Serializer.Tests
{
   public static class  AreEquals
   {
       public static void Compounds(Compound x1,Compound x2)
       {
          if (AreBothNull(x1, x2)) return;
          x1.Name.ShouldBeEqualTo(x2.Name);
          x1.Id.ShouldBeEqualTo(x2.Id);
          x1.CompoundType.ShouldBeEqualTo(x2.CompoundType);
     
       }

       public static void Projects(Project x1, Project x2)
       {
          if (AreBothNull(x1, x2)) return;
          x1.Id.ShouldBeEqualTo(x2.Id);
          x1.Name.ShouldBeEqualTo(x2.Name);
          Compounds(x1.Compound,x2.Compound);
       }

      public static void Applications(Application x1, Application x2)
      {
         if (AreBothNull(x1, x2)) return;
         x1.Id.ShouldBeEqualTo(x2.Id);
         Formulations(x1.Formulation, x2.Formulation);
         Formulations(x1.AnotherFormulation, x2.AnotherFormulation);
      }

      public static void Formulations(IFormulation x1, IFormulation x2)
      {
         if(AreBothNull(x1, x2)) return;
         x1.Id.ShouldBeEqualTo(x2.Id);
      }

      public static bool AreBothNull(object x1, object x2)
      {
         if(x1 ==null)
            x2.ShouldBeNull();

         if(x2==null)
            x1.ShouldBeNull();

         return x1 == null;
      }

      public static void Containers(Container x1, Container x2)
      {
         if (AreBothNull(x1, x2)) return;

         x1.Id.ShouldBeEqualTo(x2.Id);
         x1.Children.Count().ShouldBeEqualTo(x2.Children.Count());
         foreach (var e1 in x1.Children)
         {
            var e2 = x2.Children.FirstOrDefault(x => string.Equals(e1.Id, x.Id));
            Entities(e1,e2);
         }
      }

      public static void Entities(IEntity x1, IEntity x2)
      {
         if (AreBothNull(x1, x2)) return;
         x1.Id.ShouldBeEqualTo(x2.Id);
      }

      public static void Models(Model x1, Model x2)
      {
         if (AreBothNull(x1, x2)) return;
         x1.Name.ShouldBeEqualTo(x2.Name);
         Entities(x1.Entity,x2.Entity);
         Containers(x1.Root, x2.Root);
      }

      public static void Units(Unit x1, Unit x2)
      {
         if (AreBothNull(x1, x2)) return;
         x1.Name.ShouldBeEqualTo(x2.Name);
         x1.Color.ShouldBeEqualTo(x2.Color);
      }
   }
}