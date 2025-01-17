using System.Linq;
using System;
using System.Reflection;
using HotChocolate.Language;
using HotChocolate.Resolvers;
using HotChocolate.Types.Descriptors.Definitions;

namespace HotChocolate.Types.Descriptors
{
    public class ObjectFieldDescriptor
        : OutputFieldDescriptorBase<ObjectFieldDefinition>
        , IObjectFieldDescriptor
    {
        private bool _argumentsInitialized;

        public ObjectFieldDescriptor(
            IDescriptorContext context,
            NameString fieldName)
            : base(context)
        {
            Definition.Name = fieldName.EnsureNotEmpty(nameof(fieldName));
        }

        public ObjectFieldDescriptor(
            IDescriptorContext context,
            MemberInfo member)
            : this(context, member, null)
        {
        }

        public ObjectFieldDescriptor(
            IDescriptorContext context,
            MemberInfo member,
            Type resolverType)
            : base(context)
        {
            Definition.Member = member
                ?? throw new ArgumentNullException(nameof(member));

            Definition.Name = context.Naming.GetMemberName(
                member, MemberKind.ObjectField);
            Definition.Description = context.Naming.GetMemberDescription(
                member, MemberKind.ObjectField);
            Definition.Type = context.Inspector.GetOutputReturnType(member);
            Definition.ResolverType = resolverType;
            Definition.DeprecationReason =
                context.Naming.GetDeprecationReason(member);

            if (member is MethodInfo m)
            {
                Parameters = m.GetParameters().ToDictionary(
                    t => new NameString(t.Name));
            }
        }

        protected override ObjectFieldDefinition Definition { get; } =
            new ObjectFieldDefinition();

        protected override void OnCreateDefinition(
            ObjectFieldDefinition definition)
        {
            CompleteArguments(definition);
        }

        private void CompleteArguments(ObjectFieldDefinition definition)
        {
            if (!_argumentsInitialized)
            {
                FieldDescriptorUtilities.DiscoverArguments(
                    Context,
                    definition.Arguments,
                    definition.Member);
                _argumentsInitialized = true;
            }
        }

        public new IObjectFieldDescriptor SyntaxNode(
            FieldDefinitionNode fieldDefinition)
        {
            base.SyntaxNode(fieldDefinition);
            return this;
        }

        public new IObjectFieldDescriptor Name(NameString value)
        {
            base.Name(value);
            return this;
        }

        public new IObjectFieldDescriptor Description(
            string value)
        {
            base.Description(value);
            return this;
        }

        public new IObjectFieldDescriptor DeprecationReason(
            string value)
        {
            base.DeprecationReason(value);
            return this;
        }

        public new IObjectFieldDescriptor Type<TOutputType>()
            where TOutputType : IOutputType
        {
            base.Type<TOutputType>();
            return this;
        }

        public new IObjectFieldDescriptor Type<TOutputType>(
            TOutputType outputType)
            where TOutputType : class, IOutputType
        {
            base.Type(outputType);
            return this;
        }

        public new IObjectFieldDescriptor Type(ITypeNode typeNode)
        {
            base.Type(typeNode);
            return this;
        }

        public new IObjectFieldDescriptor Argument(
            NameString name,
            Action<IArgumentDescriptor> argument)
        {
            base.Argument(name, argument);
            return this;
        }

        public new IObjectFieldDescriptor Ignore()
        {
            base.Ignore();
            return this;
        }

        public IObjectFieldDescriptor Resolver(
            FieldResolverDelegate fieldResolver)
        {
            if (fieldResolver == null)
            {
                throw new ArgumentNullException(nameof(fieldResolver));
            }

            Definition.Resolver = fieldResolver;
            return this;
        }

        public IObjectFieldDescriptor Resolver(
            FieldResolverDelegate fieldResolver,
            Type resultType)
        {
            if (fieldResolver == null)
            {
                throw new ArgumentNullException(nameof(fieldResolver));
            }

            Definition.Resolver = fieldResolver;

            if (resultType != null)
            {
                Definition.SetMoreSpecificType(resultType, TypeContext.Output);
            }
            return this;
        }

        public IObjectFieldDescriptor Use(FieldMiddleware middleware)
        {
            if (middleware == null)
            {
                throw new ArgumentNullException(nameof(middleware));
            }

            Definition.MiddlewareComponents.Add(middleware);
            return this;
        }

        public new IObjectFieldDescriptor Directive<T>(T directive)
            where T : class
        {
            base.Directive(directive);
            return this;
        }

        public new IObjectFieldDescriptor Directive<T>()
            where T : class, new()
        {
            base.Directive<T>();
            return this;
        }

        public new IObjectFieldDescriptor Directive(
            NameString name,
            params ArgumentNode[] arguments)
        {
            base.Directive(name, arguments);
            return this;
        }

        public static ObjectFieldDescriptor New(
            IDescriptorContext context,
            NameString fieldName) =>
            new ObjectFieldDescriptor(context, fieldName);

        public static ObjectFieldDescriptor New(
            IDescriptorContext context,
            MemberInfo member) =>
            new ObjectFieldDescriptor(context, member);

        public static ObjectFieldDescriptor New(
            IDescriptorContext context,
            MemberInfo member,
            Type resolverType) =>
            new ObjectFieldDescriptor(context, member, resolverType);
    }
}
