using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace LearnSmartCoding.EssentialProducts.API.Validation
{
    public abstract class AbstractValidatableObject : IValidatableObject
    {
        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            CancellationTokenSource source = new CancellationTokenSource();

            var task = ValidateAsync(validationContext, source.Token);

            Task.WaitAll(task);

            return task.Result;
        }

        public virtual Task<IEnumerable<ValidationResult>> ValidateAsync(ValidationContext validationContext, CancellationToken cancellation)
        {
            return Task.FromResult((IEnumerable<ValidationResult>)new List<ValidationResult>());
        }

    }
}
