using AutoMapper;
using Domain.Contracts;
using Domain.Entities.BasketModule;
using Domain.Exceptions;
using Sevices.Abstraction.Contracts;
using Shared.Dtos.BasketModule;

namespace Services.Implementations
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository _repository;
        private readonly IMapper _mapper;

        public BasketService(IBasketRepository repository, IMapper mapper)
        {
           _repository = repository;
           _mapper = mapper;
        }
        public async Task<BasketDto> CreateOrUpdateBasketAsync(BasketDto basketDto)
        {
            var basket = _mapper.Map<CustomerBasket>(basketDto);
            var createdOrUpdatedBasket = await _repository.CreateOrUpdateBasketAsync(basket);

            return createdOrUpdatedBasket is null ?
                throw new Exception("Cannot Create Or Update Basket") :
                _mapper.Map<BasketDto>(createdOrUpdatedBasket);

        }

        public async Task<bool> DeleteBasketAsync(string id)
        => await _repository.DeleteBasketAsync(id);

        public async Task<BasketDto> GetBasketAsync(string id)
        {
           var basket = await _repository.GetBasketAsync(id);

            return basket is null ?
                 throw new BasketNotFoundException(id) :
                 _mapper.Map<BasketDto>(basket);
        }
    }
}
