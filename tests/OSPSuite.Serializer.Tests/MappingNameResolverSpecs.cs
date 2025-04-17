using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;

namespace OSPSuite.Serializer.Tests
{
   public abstract class concern_for_mapping_name_resolver : ContextSpecification<IMappingNameResolver>
   {
      protected override void Context()
      {
         sut = new MappingNameResolver();
      }
   }

   public class When_resolving_the_mapping_name_for_a_given_name : concern_for_mapping_name_resolver
   {
      [Observation]
      public void should_remove_all_naming_convention_from_the_given_name()
      {
         sut.MappingNameFor("ProjectManager").ShouldBeEqualTo("ProjectManager");
      }

      [Observation]
      public void should_remove_camel_case_or_hungarian_notation_at_the_beginning_of_the_given_name()
      {
         sut.MappingNameFor("_projectManager").ShouldBeEqualTo("projectManager");
         sut.MappingNameFor("m_ProjectManager").ShouldBeEqualTo("ProjectManager");
         sut.MappingNameFor("m_A").ShouldBeEqualTo("A");
         sut.MappingNameFor("_version").ShouldBeEqualTo("version");
         sut.MappingNameFor("<TOTO>k__BackingField").ShouldBeEqualTo("TOTO");
      }
   }

   public class When_resolving_the_mapping_name_for_an_invalid_input : concern_for_mapping_name_resolver
   {
      [Observation]
      public void should_return_the_input()
      {
         sut.MappingNameFor("m_").ShouldBeEqualTo("m_");
         sut.MappingNameFor("_").ShouldBeEqualTo("_");
         sut.MappingNameFor("").ShouldBeEqualTo("");
      }
   }
}