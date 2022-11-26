using Application.Specifications.Base;
using Domain.Entities;

namespace Application.Specifications
{
    public class CustomFolderFilterSpecification : Specification<CustomFolder>
    {
        public CustomFolderFilterSpecification(string searchString)
        {
            Includes.Add(a => a.Category);
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => p.Id != 0 && (string.IsNullOrWhiteSpace(searchString) ||
                p.Category.Name.Contains(searchString) ||
                p.ColorHex.Contains(searchString) ||
                p.Name.Contains(searchString));
            }
            else
                Criteria = p => p.Id != 0;
        }
    }
}
