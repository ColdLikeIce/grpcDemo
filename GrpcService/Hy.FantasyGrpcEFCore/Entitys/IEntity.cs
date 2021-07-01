using System;
using System.Collections.Generic;
using System.Text;

namespace Hy.FantasyGrpcEFCore.Entitys
{
    /// <summary>
    /// A shortcut of <see cref="IEntity{TPrimaryKey}"/> for most used primary key type (<see cref="long"/>).
    /// </summary>
    public interface IEntity : IEntity<int>
    {
    }
}