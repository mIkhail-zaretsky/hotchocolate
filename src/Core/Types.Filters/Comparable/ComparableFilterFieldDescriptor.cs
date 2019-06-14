using System.Collections.Generic;
using System.Reflection;
using HotChocolate.Types.Descriptors;

namespace HotChocolate.Types.Filters
{
    // TODO : we also need not equals
    public class ComparableFilterFieldDescriptor
        : FilterFieldDescriptorBase
        , IComparableFilterFieldDescriptor
    {
        public ComparableFilterFieldDescriptor(
            IDescriptorContext context,
            PropertyInfo property)
            : base(context, property)
        {
            AllowedOperations = new HashSet<FilterOperationKind>
            {
                FilterOperationKind.Equals, 
                FilterOperationKind.In,
                FilterOperationKind.GreaterThan,
                FilterOperationKind.GreaterThanOrEqual,
                FilterOperationKind.LowerThan,
                FilterOperationKind.LowerThanOrEqual
            };
        }

        protected override ISet<FilterOperationKind> AllowedOperations { get; }

        public IComparableFilterOperationDescriptor AllowEquals()
        {
            ComparableFilterOperationDescriptor field =
                CreateOperation(FilterOperationKind.Equals);
            Filters.Add(field);
            return field;
        }

        public IComparableFilterOperationDescriptor AllowIn()
        {
            ComparableFilterOperationDescriptor field =
                CreateOperation(FilterOperationKind.In);
            Filters.Add(field);
            return field;
        }

        public IComparableFilterOperationDescriptor AllowGreaterThan()
        {
            ComparableFilterOperationDescriptor field =
                 CreateOperation(FilterOperationKind.GreaterThan);
            Filters.Add(field);
            return field;
        }

        public IComparableFilterOperationDescriptor AllowGreaterThanOrEquals()
        {
            ComparableFilterOperationDescriptor field =
                 CreateOperation(FilterOperationKind.GreaterThanOrEqual);
            Filters.Add(field);
            return field;
        }

        public IComparableFilterOperationDescriptor AllowLowerThan()
        {
            ComparableFilterOperationDescriptor field =
                 CreateOperation(FilterOperationKind.LowerThan);
            Filters.Add(field);
            return field;
        }

        public IComparableFilterOperationDescriptor AllowLowerThanOrEquals()
        {
            ComparableFilterOperationDescriptor field =
                 CreateOperation(FilterOperationKind.LowerThanOrEqual);
            Filters.Add(field);
            return field;
        }

        public new IComparableFilterFieldDescriptor BindFilters(
            BindingBehavior bindingBehavior)
        {
            base.BindFilters(bindingBehavior);
            return this;
        }

        public IComparableFilterFieldDescriptor BindExplicitly() =>
            BindFilters(BindingBehavior.Explicit);

        public IComparableFilterFieldDescriptor BindImplicitly() =>
            BindFilters(BindingBehavior.Explicit);

        protected override FilterOperationDefintion CreateOperationDefinition(
            FilterOperationKind operationKind) =>
            CreateOperation(operationKind).CreateDefinition();

        private ComparableFilterOperationDescriptor CreateOperation(
            FilterOperationKind operationKind)
        {
            var operation = new FilterOperation(
                typeof(string),
                operationKind,
                Definition.Property);

            return ComparableFilterOperationDescriptor.New(
                Context,
                this,
                CreateFieldName(operationKind),
                RewriteType(operationKind),
                operation);
        }
    }
}
