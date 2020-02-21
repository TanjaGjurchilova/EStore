using System;
using System.Collections.Generic;
using System.Linq;
using EStore.Messages;
using EStore.Messages.Request.Cart;
using EStore.Messages.Response.Cart;
using EStore.Models.Cart;
using EStore.Repositories;
using Microsoft.AspNetCore.Http;

namespace EStore.Services.Implementations
{
    public class CartService : ICartService
    {
        private const string UniqueCartIdSessionKey = "UniqueCartId";
        private readonly ICartRepository _cartRepository;
        private readonly ICartItemRepository _cartItemRepository;
        private MessageMapper _messageMapper;
        private readonly HttpContext _httpContext;
        private readonly IProductRepository _productRepository;
        public CartService(
            IHttpContextAccessor httpContextAccessor,
            ICartRepository cartRepository,
            ICartItemRepository cartItemRepository,
            IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
            _messageMapper = new MessageMapper();
            _httpContext = httpContextAccessor.HttpContext;
            _productRepository = productRepository;
        }

        public AddItemToCartResponse AddItemToCart(AddItemToCartRequest addItemToCartRequest)
        {
            AddItemToCartResponse response = new AddItemToCartResponse();

            var cart = GetCart();

            if (cart != null)
            {
                var existingCartItem = _cartItemRepository.FindCartItemsByCartId(cart.Id)
                                     .FirstOrDefault(c => c.ProductId == addItemToCartRequest.ProductId);
                if (existingCartItem != null)
                {
                    existingCartItem.Quantity++;
                    _cartItemRepository.UpdateCartItem(existingCartItem);

                    response.CartItem = _messageMapper.MapToCartItemDto(existingCartItem);
                }
                else
                {
                    var product = _productRepository.FindProductById(addItemToCartRequest.ProductId);

                    if (product != null)
                    {
                        var cartItem = new CartItem
                        {
                            CartId = cart.Id,
                            Cart = cart,
                            ProductId = addItemToCartRequest.ProductId,
                            Product = product,
                            Quantity = 1
                        };
                        _cartItemRepository.SaveCartItem(cartItem);
                        response.CartItem = _messageMapper.MapToCartItemDto(cartItem);
                    }
                }
            }
            else
            {
                var product = _productRepository.FindProductById(addItemToCartRequest.ProductId);
                if (product != null)
                {
                    var newCart = new Cart
                    {
                        UniqueCartId = UniqueCartId(),
                        CartStatus = CartStatus.Open
                    };

                    _cartRepository.SaveCart(newCart);

                    var cartItem = new CartItem
                    {
                        CartId = newCart.Id,
                        Cart = newCart,
                        ProductId = addItemToCartRequest.ProductId,
                        Product = product,
                        Quantity = 1
                    };

                    _cartItemRepository.SaveCartItem(cartItem);

                    response.CartItem = _messageMapper.MapToCartItemDto(cartItem);

                }
            }
            return response;
        }

        public RemoveItemFromCartResponse RemoveItemFromCart(RemoveItemFromCartRequest removeItemFromCartRequest)
        {
            RemoveItemFromCartResponse response = new RemoveItemFromCartResponse();
            var cartItem = _cartItemRepository.FindCartItemById(removeItemFromCartRequest.CartItemId);
            _cartItemRepository.DeleteCartItem(cartItem);

            response.CartItemId = cartItem.Id;
            return response;
        }

        public string UniqueCartId()
        {
            if (!string.IsNullOrWhiteSpace(_httpContext.Session.GetString(UniqueCartIdSessionKey)))
            {
                return _httpContext.Session.GetString(UniqueCartIdSessionKey);
            }
            else
            {
                var uniqueId = Guid.NewGuid().ToString();
                _httpContext.Session.SetString(UniqueCartIdSessionKey, uniqueId);

                return _httpContext.Session.GetString(UniqueCartIdSessionKey);
            }
        }

        public Cart GetCart()
        {
            var uniqueId = UniqueCartId();
            var cart = _cartRepository.GetAllCarts().FirstOrDefault(c => c.UniqueCartId == uniqueId);
            return cart;
        }

        public FetchCartResponse FetchCart()
        {
            FetchCartResponse response = new FetchCartResponse();
            var cart = GetCart();
            IList<CartItem> cartItems = new List<CartItem>();

            if (cart != null)
            {
                cartItems = _cartItemRepository.FindCartItemsByCartId(cart.Id).ToArray();
                var cartItemsDto = _messageMapper.MapToCartItemDtos(cartItems);
                var cartDto = _messageMapper.MapToCartDto(cart);
                cartDto.CartItems = cartItemsDto;
                response.Cart = cartDto;
            }

            return response;
        }

        public IEnumerable<CartItem> GetCartItems()
        {
            IList<CartItem> cartItems = new List<CartItem>();

            var cart = GetCart();

            if (cart != null)
            {
                cartItems = _cartItemRepository.FindCartItemsByCartId(cart.Id).ToArray();
            }

            return cartItems;
        }

        public int CartItemsCount()
        {

            var count = 0;
            var cartItems = GetCartItems();

            foreach (var cartItem in cartItems)
            {
                count += cartItem.Quantity;
            }

            return count;
        }

        public decimal GetCartTotal()
        {
            decimal total = 0;

            var cartItems = GetCartItems();

            foreach (var cartItem in cartItems)
            {
                var product = _productRepository.FindProductById(cartItem.ProductId);
                total = total + (cartItem.Quantity * product.Price);
            }

            return total;
        }
    }
}
