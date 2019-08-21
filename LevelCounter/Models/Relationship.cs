﻿using System.Collections.Generic;

namespace LevelCounter.Models
{
    public class Relationship
    {
        public int RelationshipId { get; set; }
        public ApplicationUser User { get; set; }
        public ApplicationUser RelatingUser { get; set; }
        public string State
        {
            get
            {
                switch (RelationshipState)
                {
                    case (RelationshipStates.BLOCKED):
                        return "blocked";
                    case (RelationshipStates.CONFIRMED):
                        return "confirmed";
                    case (RelationshipStates.PENDING):
                        return "pending";
                }
                return "Invalid gender type";
            }
        }
        public RelationshipStates RelationshipState { get; set; }
        public enum RelationshipStates
        {
            BLOCKED = 0,
            CONFIRMED = 1,
            PENDING = 2
        }
        public IEnumerable<UserRelationships> Relationships { get; set; }
    }
}
