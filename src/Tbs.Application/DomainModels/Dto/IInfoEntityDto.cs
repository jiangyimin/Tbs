using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using Tbs.DomainModels;

namespace Tbs.DomainModels.Dto
{
    /// <summary>
    /// A shortcut of <see cref="ICrudEntityDto{TPrimaryKey}"/> for most used primary key type (<see cref="int"/>).
    /// </summary>
    public interface IInfoEntityDto : IInfoEntityDto<int>
    {
    }

    /// <summary>
    /// Defines common properties for entity based DTOs.
    /// </summary>
    /// <typeparam name="TPrimaryKey"></typeparam>
    public interface IInfoEntityDto<TPrimaryKey> : IEntityDto<TPrimaryKey>
    {
        /// <summary>
        /// Info of the entity.
        /// </summary>
        string Info { get; }
        void Convert();
    }
}