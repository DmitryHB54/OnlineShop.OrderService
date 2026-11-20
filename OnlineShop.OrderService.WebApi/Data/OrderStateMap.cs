using MassTransit;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShop.OrderService.WebApi.Contracts;
using OnlineShop.OrderService.WebApi.Saga;

namespace OnlineShop.OrderService.WebApi.Data;

public class OrderStateMap : SagaClassMap<OrderState>
{
    protected override void Configure(EntityTypeBuilder<OrderState> entity, ModelBuilder modelBuilder)
    {
        entity.ToTable("order_sagas");
        entity.Property(x => x.CurrentState).HasMaxLength(64);

        entity.Property(x => x.Items)
            .HasColumnType("jsonb")
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<List<OrderItemDto>>
                    (v, (JsonSerializerOptions?)null) ?? new()
            );
    }
}