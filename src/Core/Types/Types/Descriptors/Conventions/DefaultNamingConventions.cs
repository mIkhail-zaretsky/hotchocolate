using System;
using System.Linq;
using System.Reflection;
using HotChocolate.Utilities;

namespace HotChocolate.Types.Descriptors
{
    public class DefaultNamingConventions
        : INamingConventions
    {
        private IDocumentationProvider _documentation;

        public DefaultNamingConventions(IDocumentationProvider documentation)
        {
            _documentation = documentation
                ?? throw new ArgumentNullException(nameof(documentation));
        }

        public DefaultNamingConventions()
        {
            _documentation = new XmlDocumentationProvider(
                new XmlDocumentationFileResolver());
        }

        protected IDocumentationProvider DocumentationProvider =>
            _documentation;

        public virtual NameString GetArgumentName(ParameterInfo parameter)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }
            return parameter.GetGraphQLName();
        }

        public virtual string GetArgumentDescription(ParameterInfo parameter)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            var description = parameter.GetGraphQLDescription();
            if (string.IsNullOrWhiteSpace(description))
            {
                description = _documentation.GetSummary(parameter);
            }

            return description;
        }

        public virtual NameString GetEnumValueName(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return value.ToString().ToUpperInvariant();
        }

        public virtual string GetEnumValueDescription(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            Type enumType = value.GetType();
            if (enumType.IsEnum)
            {
                MemberInfo enumMember = enumType
                    .GetMember(value.ToString())
                    .FirstOrDefault();

                if (enumMember != null)
                {
                    string description = enumMember.GetGraphQLDescription();
                    if (string.IsNullOrEmpty(description))
                    {
                        return _documentation.GetSummary(enumMember);
                    }
                    return description;
                }
            }

            return null;
        }

        public virtual NameString GetMemberName(
            MemberInfo member,
            MemberKind kind)
        {
            if (member == null)
            {
                throw new ArgumentNullException(nameof(member));
            }

            return member.GetGraphQLName();
        }

        public virtual string GetMemberDescription(
            MemberInfo member,
            MemberKind kind)
        {
            if (member == null)
            {
                throw new ArgumentNullException(nameof(member));
            }

            var description = member.GetGraphQLDescription();
            if (string.IsNullOrWhiteSpace(description))
            {
                description = _documentation.GetSummary(member);
            }

            return description;
        }

        public virtual NameString GetTypeName(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return type.GetGraphQLName();
        }

        public virtual NameString GetTypeName(Type type, TypeKind kind)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            string name = type.GetGraphQLName();

            if (kind == TypeKind.InputObject
                && !name.EndsWith("Input", StringComparison.Ordinal))
            {
                name = name + "Input";
            }

            return name;
        }

        public virtual string GetTypeDescription(Type type, TypeKind kind)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            var description = type.GetGraphQLDescription();
            if (string.IsNullOrWhiteSpace(description))
            {
                description = _documentation.GetSummary(type);
            }

            return description;
        }

        public virtual string GetDeprecationReason(MemberInfo member)
        {
            if (member == null)
            {
                throw new ArgumentNullException(nameof(member));
            }

            return member.GetGraphQLDeprecationReason();
        }

        public virtual string GetDeprecationReason(object value)
        {
            Type enumType = value.GetType();

            if (enumType.IsEnum)
            {
                MemberInfo enumMember = enumType
                    .GetMember(value.ToString())
                    .FirstOrDefault();

                if (enumMember != null)
                {
                    return enumMember.GetGraphQLDeprecationReason();
                }
            }

            return null;
        }
    }
}
