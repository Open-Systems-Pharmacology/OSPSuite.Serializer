using System;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Serializer.Reflection;

namespace OSPSuite.Serializer.Tests
{
   public abstract class concern_for_member_accessor_factory : ContextSpecification<IMemberAccessorFactory>
   {
      protected Project _project;

      protected override void Context()
      {
         sut = new MemberAccessorFactory();
         _project = new Project(5);
      }
   }

   public class When_creating_a_member_accessor_for_a_property_that_is_read_write : concern_for_member_accessor_factory
   {
      private IMemberAccessor _result;

      protected override void Because()
      {
         _result = sut.CreateFor<Project, string>(x => x.Name);
      }

      [Observation]
      public void should_return_a_property_accessor()
      {
         _result.ShouldBeAnInstanceOf<ReadWritePropertyAccessor>();
      }

      [Observation]
      public void should_be_able_to_set_a_value_to_the_property()
      {
         _result.SetValue(_project, "asd");
      }

      [Observation]
      public void should_be_able_to_read_a_value_of_the_property()
      {
         _result.GetValue(_project);
      }

      [Observation]
      public void should_be_able_to_return_the_type_of_the_property()
      {
         _result.MemberType.ShouldBeEqualTo(typeof(string));
      }
   }

   public class When_creating_a_member_accessor_for_a_public_field : concern_for_member_accessor_factory
   {
      private IMemberAccessor _result;

      protected override void Because()
      {
         _result = sut.CreateFor<Project, string>(x => x.Description);
      }

      [Observation]
      public void should_return_a_field_accessor()
      {
         _result.ShouldBeAnInstanceOf<FieldAccessor>();
      }

      [Observation]
      public void should_be_able_to_set_a_value_to_the_property()
      {
         _result.SetValue(_project, "tralala");
      }

      [Observation]
      public void should_be_able_to_read_a_value_of_the_property()
      {
         _result.GetValue(_project);
      }
   }

   public class When_creating_a_member_accessor_for_a_readonly_property_with_backing_field_in_camel_case : concern_for_member_accessor_factory
   {
      private IMemberAccessor _result;

      protected override void Because()
      {
         _result = sut.CreateFor<Project, string>(x => x.Id);
      }

      [Observation]
      public void should_return_a_field_accessor()
      {
         _result.ShouldBeAnInstanceOf<ReadOnlyPropertyWithPrivateFieldAccessor>();
      }

      [Observation]
      public void should_be_able_to_set_a_value_to_the_property()
      {
         _result.SetValue(_project, "newid");
      }

      [Observation]
      public void should_be_able_to_read_a_value_of_the_property()
      {
         _result.GetValue(_project);
      }
   }

   public class When_creating_a_member_accessor_for_a_readonly_property_with_backing_field_in_lowercase : concern_for_member_accessor_factory
   {
      private IMemberAccessor _result;

      protected override void Because()
      {
         _result = sut.CreateFor<Project, string>(x => x.LowCaseProperty);
      }

      [Observation]
      public void should_return_a_field_accessor()
      {
         _result.ShouldBeAnInstanceOf<ReadOnlyPropertyWithPrivateFieldAccessor>();
      }

      [Observation]
      public void should_be_able_to_set_a_value_to_the_property()
      {
         _result.SetValue(_project, "newid");
      }

      [Observation]
      public void should_be_able_to_read_a_value_of_the_property()
      {
         _result.GetValue(_project);
      }
   }

   public class When_creating_a_member_accessor_for_a_readonly_property_with_backing_field_in_hungarian_style_and_lower_case : concern_for_member_accessor_factory
   {
      private IMemberAccessor _result;

      protected override void Because()
      {
         _result = sut.CreateFor<Project, string>(x => x.ProjectManager);
      }

      [Observation]
      public void should_return_a_read_only_property_accessor()
      {
         _result.ShouldBeAnInstanceOf<ReadOnlyPropertyWithPrivateFieldAccessor>();
      }

      [Observation]
      public void should_be_able_to_set_a_value_to_the_property()
      {
         _result.SetValue(_project, "new project manager");
      }

      [Observation]
      public void should_be_able_to_read_a_value_of_the_property()
      {
         _result.GetValue(_project);
      }
   }

   public class When_creating_a_member_accessor_for_a_property_without_backing_field : concern_for_member_accessor_factory
   {
      private IMemberAccessor _result;

      protected override void Because()
      {
         _result = sut.CreateFor<Project, string>(x => x.PropertyWithoutBackingField);
      }

      [Observation]
      public void should_return_a_read_only_property_accessor()
      {
         _result.ShouldBeAnInstanceOf<ReadOnlyPropertyAccessor>();
      }

      [Observation]
      public void should_throw_an_exception_when_setting_a_value_to_the_property()
      {
         The.Action(() => _result.SetValue(_project, "new project manager")).ShouldThrowAn<MemberAccessException>();
      }

      [Observation]
      public void should_be_able_to_read_a_value_of_the_property()
      {
         _result.GetValue(_project);
      }
   }

   public class When_creating_a_member_accessor_for_an_auto_property_read_only : concern_for_member_accessor_factory
   {
      private IMemberAccessor _result;

      protected override void Because()
      {
         _result = sut.CreateFor<Project, string>(x => x.AutoPropertyReadOnly);
      }

      [Observation]
      public void should_return_a_read_write_property_accessor()
      {
         _result.ShouldBeAnInstanceOf<ReadWritePropertyAccessor>();
      }

      [Observation]
      public void should_be_able_to_set_a_value()
      {
         _result.SetValue(_project, "new project manager");
      }

      [Observation]
      public void should_be_able_to_read_a_value_of_the_property()
      {
         _result.GetValue(_project);
      }
   }

   public class When_creating_a_member_accessor_for_an_auto_property_read_only_with_a_property_set_protected : concern_for_member_accessor_factory
   {
      private IMemberAccessor _result;

      protected override void Because()
      {
         _result = sut.CreateFor<Project, string>(x => x.AutoPropertyProtectedSet);
      }

      [Observation]
      public void should_return_a_read_write_property_accessor()
      {
         _result.ShouldBeAnInstanceOf<ReadWritePropertyAccessor>();
      }

      [Observation]
      public void should_be_able_to_set_a_value()
      {
         _result.SetValue(_project, "new project manager");
      }

      [Observation]
      public void should_be_able_to_read_a_value_of_the_property()
      {
         _result.GetValue(_project);
      }
   }

   public class When_creating_a_member_accessor_for_an_auto_property_read_only_without_setter : concern_for_member_accessor_factory
   {
      private IMemberAccessor _result;

      protected override void Because()
      {
         _result = sut.CreateFor<Project, string>(x => x.AutoPropertyReadOnlyNoSetter);
      }

      [Observation]
      public void should_return_a_read_write_property_accessor()
      {
         _result.ShouldBeAnInstanceOf<ReadOnlyPropertyWithPrivateFieldAccessor>();
      }

      [Observation]
      public void should_be_able_to_set_a_value()
      {
         _result.SetValue(_project, "new project manager");
      }

      [Observation]
      public void should_be_able_to_read_a_value_of_the_property()
      {
         _result.GetValue(_project);
      }
   }

   public class When_creating_a_member_accessor_for_a_method_or_a_function : concern_for_member_accessor_factory
   {
      [Observation]
      public void should_throw_an_exception()
      {
         The.Action(() => sut.CreateFor<Project, string>(x => x.DoSomething())).ShouldThrowAn<Exception>();
      }
   }

   public class When_creating_a_member_accessor_for_a_private_field : concern_for_member_accessor_factory
   {
      private IMemberAccessor _result;

      protected override void Because()
      {
         _result = sut.CreateFor<Project, int>("_version");
      }

      [Observation]
      public void should_return_a_field_accessor()
      {
         _result.ShouldBeAnInstanceOf<FieldAccessor>();
      }

      [Observation]
      public void should_be_able_to_set_a_value_to_the_property()
      {
         _result.SetValue(_project, 2);
      }

      [Observation]
      public void should_be_able_to_read_a_value_of_the_property()
      {
         _result.GetValue(_project);
      }
   }

   public class When_creating_a_member_accessor_for_a_protected_field : concern_for_member_accessor_factory
   {
      private IMemberAccessor _result;

      protected override void Because()
      {
         _result = sut.CreateFor<Project, int>("_protectedField");
      }

      [Observation]
      public void should_return_a_field_accessor()
      {
         _result.ShouldBeAnInstanceOf<FieldAccessor>();
      }

      [Observation]
      public void should_be_able_to_set_a_value_to_the_property()
      {
         _result.SetValue(_project, 2);
      }

      [Observation]
      public void should_be_able_to_read_a_value_of_the_property()
      {
         _result.GetValue(_project);
      }
   }

   public class When_creating_a_member_accessor_for_a_protected_readonly_field : concern_for_member_accessor_factory
   {
      [Observation]
      public void should_throw_an_exception()
      {
         The.Action(() => sut.CreateFor<Project, int>("_protectedReadOnlyField")).ShouldThrowAn<MemberAccessException>();
      }
   }

   public class When_creating_a_member_accessor_for_a_private_field_that_does_not_exist : concern_for_member_accessor_factory
   {
      [Observation]
      public void should_throw_an_exception()
      {
         The.Action(() => sut.CreateFor<Project, int>("toto")).ShouldThrowAn<MemberAccessException>();
      }
   }

   public class When_creating_a_member_accessor_for_a_private_field_that_does_exit_but_with_the_wrong_type : concern_for_member_accessor_factory
   {
      [Observation]
      public void should_throw_an_exception()
      {
         The.Action(() => sut.CreateFor<Project, string>("_version")).ShouldThrowAn<MemberAccessException>();
      }
   }
}