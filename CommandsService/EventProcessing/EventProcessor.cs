using System;
using System.Text.Json;
using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using Microsoft.Extensions.DependencyInjection;

namespace CommandsService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }
        
        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch (eventType)
            {
                case EventType.PlatformPublished:
                    addPlatform(message);
                    break;
                default:
                    break;
            }
        }

        private static EventType DetermineEvent(string notificationMessage)
        {
            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);

            return eventType?.Event switch
            {
                "Platform_Published" => EventType.PlatformPublished,
                _ => EventType.Undetermined
            };
        }

        private void addPlatform(string platformPublishedMessage)
        {
            using var scope = _scopeFactory.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();

            var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishedDto>(platformPublishedMessage);

            try
            {
                var platform = _mapper.Map<Platform>(platformPublishedDto);
                if (repo.ExternalPlatformExist(platform.ExternalId)) return;
                    
                repo.CreatePlatform(platform);
                repo.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not add Platform to DB {ex.Message}");
            }
        }
    }

    internal enum EventType
    {
        PlatformPublished,
        Undetermined
    }
}