# 🛒 E-Commerce Web API

A production-ready, scalable RESTful API for a full-featured e-commerce platform built with **ASP.NET Core**, following **Clean Architecture** principles and modern software design patterns.

---

## 🚀 Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core Web API |
| ORM | Entity Framework Core |
| Database | SQL Server |
| Identity | ASP.NET Core Identity |
| Caching | Redis |
| Authentication | JWT (JSON Web Tokens) |
| Object Mapping | AutoMapper |
| API Docs | Swagger / OpenAPI |

---

## 🏗️ Architecture — Clean Architecture (7 Projects)

The solution is structured into **7 separate projects**, each with a single responsibility:

```
E-Commerce Solution
├── 📦 Core/
│   ├── Domain                    # Entities, Interfaces, Contracts
│   └── Services.Abstraction      # Service interfaces (IProductService, IOrderService, ...)
│
├── 📦 Infrastructure/
│   ├── Services                  # Business logic implementations + AutoMapper profiles
│   ├── Persistence               # EF Core DbContext, Repositories, Migrations, Data Seeding
│   └── Presentation              # Controllers, Attributes (RedisCacheAttribute)
│
├── 📦 Shared                     # DTOs, Error models, Result wrappers, Enums
│
└── 📦 E-Commerce.API             # Entry point — Middlewares, Extensions, Factories, DI setup
```

---

## 🧱 Domain Layer (`Core/Domain`)

### Entities

| Module | Entities |
|---|---|
| **BasketModule** | `BasketItem`, `CustomerBasket` |
| **IdentityModule** | `User`, `Address` |
| **OrderModule** | `Order`, `OrderItem`, `DeliveryMethod`, `Address`, `ProductInOrderItem` |
| **ProductModule** | `Product`, `ProductBrand`, `ProductType` |
| **Base** | `BaseEntity` |

### Contracts (Interfaces)
- `IGenericRepository<T>` — generic data access contract
- `IBasketRepository` — Redis-based basket operations
- `ICacheRepository` — caching abstraction
- `IUnitOfWork` — transaction management
- `ISpecifications<T>` — Specification Pattern contract
- `IDataSeeding` — database seeding contract

---

## ⚙️ Services Layer

### Service Implementations

| Service | Responsibility |
|---|---|
| `AuthenticationService` | Register, login, JWT token generation |
| `ProductService` | Product listing, filtering, pagination |
| `BasketService` | Cart CRUD via Redis |
| `OrderService` | Order creation and management |
| `PaymentService` | Payment intent processing |
| `CacheService` | Redis caching operations |
| `ServiceManager` | Aggregates all services (Facade Pattern) |

### AutoMapper Profiles
- `ProductProfile` — Product entity ↔ DTO
- `BasketProfile` — Basket entity ↔ DTO
- `OrderProfile` — Order entity ↔ DTO
- `PictureUrlResolver` — Resolves absolute image URLs

### Specifications
- `BaseSpecifications<T>` — Base class with filtering, ordering, includes
- `ProductWithBrandAndTypeSpecification` — Filtered product queries
- `ProductCountSpecification` — Count query for pagination
- `OrderWithIncludesSpecification` — Order with related data
- `OrderWithPaymentIntentIdSpecification` — Order lookup by payment intent

---

## 🗄️ Persistence Layer

- **`StoreDbContext`** — Main EF Core context (Products, Orders, DeliveryMethods)
- **`IdentityStoreDbContext`** — ASP.NET Core Identity context (separate DB)
- **`DataSeeding`** — Seed data for products, brands, types, and delivery methods
- **`Configurations/`** — Fluent API entity configurations
- **`Migrations/`** — Separate migrations for Store and Identity databases

### Repositories
- `GenericRepository<T>` — CRUD with Specification Pattern support
- `BasketRepository` — Redis-based basket storage
- `CacheRepository` — Redis caching implementation
- `UnitOfWork` — Coordinates multiple repositories in a transaction

---

## 🎮 Presentation Layer — API Endpoints

### 🔐 Authentication
| Method | Endpoint | Description |
|---|---|---|
| `POST` | `/api/Authentication/Register` | Register a new user |
| `POST` | `/api/Authentication/Login` | Login and receive JWT token |
| `GET` | `/api/Authentication/EmailExist` | Check if email is already registered |
| `GET` | `/api/Authentication` | Get current authenticated user info |
| `GET` | `/api/Authentication/Address` | Get user's saved address |
| `PUT` | `/api/Authentication/Address` | Update user's address |

### 🛒 Basket
| Method | Endpoint | Description |
|---|---|---|
| `GET` | `/api/Basket` | Get current user's basket |
| `POST` | `/api/Basket` | Create or update basket |
| `DELETE` | `/api/Basket/{id}` | Delete basket by ID |

### 📋 Orders
| Method | Endpoint | Description |
|---|---|---|
| `POST` | `/api/Orders` | Place a new order |
| `GET` | `/api/Orders` | Get all orders for current user |
| `GET` | `/api/Orders/{id}` | Get order by ID |
| `GET` | `/api/Orders/DeliveryMethods` | Get available delivery methods |

### 💳 Payments
| Method | Endpoint | Description |
|---|---|---|
| `POST` | `/api/Payments/{basketId}` | Create or update payment intent for a basket |

### 📦 Products
| Method | Endpoint | Description |
|---|---|---|
| `GET` | `/api/Products` | Get all products (supports filter / sort / pagination) |
| `GET` | `/api/Products/Brands` | Get all product brands |
| `GET` | `/api/Products/Types` | Get all product types |
| `GET` | `/api/Products/{id}` | Get product by ID |

**Custom Attributes**
- `RedisCacheAttribute` — Action filter that caches endpoint responses in Redis

---

## 📨 Shared Layer

- **DTOs** — Per module: `BasketModule`, `IdentityModule`, `OrderModule`, `ProductModule`
- **Result Pattern** — `Result<T>`, `Error`, `ErrorType` for consistent API responses
- **`PaginatedResult<T>`** — Generic paginated response wrapper
- **`ProductSpecificationParameters`** — Strongly-typed query params for filtering
- **`ProductSortingOptions`** enum — Sort options (PriceAsc, PriceDesc, Name)
- **`JwtOptions`** — Strongly-typed JWT configuration

---

## ✨ Key Features

### 🔐 Authentication & Authorization
- JWT Bearer token authentication
- ASP.NET Core Identity for user management
- Role-based access control

### 📦 Product Management
- Filtering by brand & type, sorting, and server-side pagination
- Specification Pattern for reusable, composable queries

### 🛒 Shopping Cart (Redis)
- Full basket CRUD stored in Redis (zero SQL overhead for cart ops)
- Basket persisted by customer ID

### 📋 Order Management
- Place orders from active basket
- Delivery method selection
- Full order history per user

### 💳 Payment Integration
- Create and update payment intents
- Payment status tracking via `OrderPaymentStatus` enum

### ⚡ Redis Response Caching
- Endpoint-level response caching via `RedisCacheAttribute`
- Configurable cache duration per endpoint

---

## 🔧 Design Patterns Used

| Pattern | Where Applied |
|---|---|
| **Clean Architecture** | Full 7-project solution structure |
| **Repository Pattern** | `GenericRepository`, `BasketRepository` |
| **Unit of Work** | `UnitOfWork` across repositories |
| **Specification Pattern** | All complex queries (Product, Order) |
| **Service Manager (Facade)** | `ServiceManager` aggregates all services |
| **DTO Pattern** | `Shared` layer DTOs per module |
| **Result Pattern** | `Result<T>`, `Error`, `ErrorType` |
| **Action Filter** | `RedisCacheAttribute` for response caching |

---



