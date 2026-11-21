using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShop.OrderService.WebApi.Contracts;
using OnlineShop.OrderService.WebApi.Saga;
using System.Text.Json;

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
            ).Metadata.SetValueComparer(new ValueComparer<List<OrderItemDto>>(
                (c1, c2) => c1!.SequenceEqual(c2!),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList()));
    }
}