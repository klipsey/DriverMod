using RobDriver.Modules;
using RobDriver.Modules.Survivors;
using RoR2;
using System.Collections.ObjectModel;
using UnityEngine;

namespace DriverMod.Modules.Components
{
    internal class MagneticPickup : MonoBehaviour
    {
        // stole this code from MagneticPickups mod
        private void FixedUpdate()
        {
            // Retrieve players and get the closest one.
            var players = TeamComponent.GetTeamMembers(TeamIndex.Player);
            var location = GetClosestPlayerLocation(players, this.transform.position);

            // Move the pickup towards the player's location if they are within the radius.
            if (Vector3.Distance(this.transform.position, location) < Config.pickupRadius.Value)
            {
                MovePickupTowardsPlayer(location);
            }
        }

        // stole this code from MagneticPickups mod
        private Vector3 GetClosestPlayerLocation(
            ReadOnlyCollection<TeamComponent> players,
            Vector3 location)
        {
            var closestPosition = Vector3.positiveInfinity;
            var lowestDistance = float.MaxValue;
            foreach (var teamComponent in players)
            {
                var networkBody = Util.LookUpBodyNetworkUser(teamComponent.gameObject);
                if (!networkBody || teamComponent.body.baseNameToken != Driver.bodyNameToken)
                {
                    continue;
                }

                var distance = Vector3.Distance(teamComponent.body.footPosition, location);
                if (distance < lowestDistance)
                {
                    closestPosition = teamComponent.body.footPosition;
                    lowestDistance = distance;
                }
            }
            return closestPosition;
        }

        // stole this code from MagneticPickups mod
        private void MovePickupTowardsPlayer(Vector3 playerLocation)
        {
            var rigidBody = this.GetComponent<Rigidbody>();

            // Set the velocity to 0 to begin with to remove any gravity.
            rigidBody.velocity = Vector3.zero;

            // Move the pickup upwards, if it is not already high above the ground and there's nothing directly above it.
            var didHitUp = Physics.Raycast(
                this.transform.position,
                this.transform.up,
                out var upwardsHit
            );
            var didHitDown = Physics.Raycast(
                this.transform.position,
                -this.transform.up,
                out var downwardsHit
            );

            var hasSpaceUpwards = !didHitUp || upwardsHit.distance > 10f;
            var hasSpaceDownwards = didHitDown && downwardsHit.distance < 250f;

            // Only move upwards if the pickup has enough space.
            if (hasSpaceUpwards && hasSpaceDownwards)
            {
                rigidBody.velocity += Vector3.up * 1.25f;
            }

            var speed = Vector3.MoveTowards(
                rigidBody.velocity,
                (this.transform.position - playerLocation).normalized * 100f,
                Config.pickupSpeed.Value
            );

            rigidBody.velocity += -speed;
        }
    }
}
