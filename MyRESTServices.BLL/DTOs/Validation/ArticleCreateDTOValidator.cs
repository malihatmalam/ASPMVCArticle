using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRESTServices.BLL.DTOs.Validation
{
    public class ArticleCreateDTOValidator : AbstractValidator<ArticleCreateDTO>
    {
        public ArticleCreateDTOValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Judul harus diisi");
            RuleFor(x => x.Title).MaximumLength(100).WithMessage("Judul maksimal 50 karakter");
            RuleFor(x => x.Details).NotEmpty().WithMessage("Detail harus diisi");
            RuleFor(x => x.IsApproved).NotEmpty().WithMessage("Approval harus diisi");
            RuleFor(x => x.CategoryID).NotEmpty().WithMessage("Category harus diisi");
        }
    }
}
