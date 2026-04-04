using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BuildingBlocks.CQRS
{
    public interface ICommand : ICommand<Unit>  //hicbir sey donmuyorum
    {
    }

    public interface ICommand<out TResponse> : IRequest<TResponse> //out = sadece return donus tipinde kullanilabilir 
    {

    }
}