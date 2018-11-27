using System;
using System.Windows;

namespace BeltRunnerEditor
{
    /// <summary>
    /// Math operations
    /// </summary>
    public static class BRMath
    {
        /// <summary>
        /// Calculates the velocity given a starting point, destination point, and speed
        /// </summary>
        /// <param name="x1">Starting X point</param>
        /// <param name="y1">Starting Y point</param>
        /// <param name="x2">Destination X point</param>
        /// <param name="y2">Destination Y point</param>
        /// <param name="speed">Speed</param>
        /// <returns>Velocity</returns>
        public static Vector CalculateVelocity(double x1, double y1,
            double x2, double y2, double speed)
        {
            // Calculate point differences
            double diffX = x2 - x1;
            double diffY = y2 - y1;

            // Calculate the distance
            double distance = Math.Sqrt(Math.Pow((diffX), 2) + Math.Pow((diffY), 2));

            // Calculate calculations of calculaty things
            double calculaty = speed / distance;

            Vector velocity = new Vector(calculaty * diffX, calculaty * diffY);
            return velocity;
        }

        /// <summary>
        /// Calculate the destination based on starting point and velocity
        /// </summary>
        /// <param name="x">X position of the entity</param>
        /// <param name="y">Y position of the entity</param>
        /// <param name="width">Width of the entity</param>
        /// <param name="height">Height of the entity</param>
        /// <param name="vx">X velocity of the entity</param>
        /// <param name="vy">Y velocity of the entity</param>
        /// <returns>Destination vector</returns>
        public static Vector CalculateDestination(double x, double y,
            double width, double height, double vx, double vy)
        {
            // Overriden points for math
            double ox = 0;
            double oy = 0;

            // Point mods
            double modx = 0;
            double mody = 0;

            // If the velocity is nothing, then the destination is the position
            if (vx == 0 && vy == 0)
            {
                return new Vector(x, y);
            }

            // Determine the method to use for X off bounds checking
            Func<double, bool> xCheck = null;
            if (vx == 0)
            {
                ox = x;
                xCheck = (v) => false;
            }
            else if (vx > 0)
            {
                modx = (width / 2) * -1;
                ox = x + modx;
                xCheck = (v) => v > 1334;
            }
            else
            {
                modx = (width / 2);
                ox = x + modx;
                xCheck = (v) => v < 0;
            }

            // Determine the method to use for Y off bounds checking
            Func<double, bool> yCheck = null;
            if (vy == 0)
            {
                oy = y;
                yCheck = (v) => false;
            }
            else if (vy > 0)
            {
                mody = (height / 2) * -1;
                oy = y + mody;
                yCheck = (v) => v > 750;
            }
            else
            {
                mody = (height / 2);
                oy = y + mody;
                yCheck = (v) => v < 0;
            }

            // Keep counting until both checks pass
            double cx = 0 + ox;
            double cy = 0 + oy;
            while (!xCheck(cx) && !yCheck(cy))
            {
                cx += vx;
                cy += vy;
            }

            return new Vector(cx - modx, cy - mody);
        }

        /// <summary>
        /// Calculates the speed given a position, destination, and velocity
        /// </summary>
        /// <param name="position">Position of the entity</param>
        /// <param name="destination">Destination of the entity</param>
        /// <param name="velocity">Velocity of the entity</param>
        /// <returns>Speed</returns>
        public static double CalculateSpeed(Vector position, Vector destination, Vector velocity)
        {
            if (velocity.X == 0 && velocity.Y == 0)
            {
                return 0;
            }

            // Calculate point differences
            double diffX = destination.X - position.X;
            double diffY = destination.Y - position.Y;

            // Calculate the distance
            double distance = Math.Sqrt(Math.Pow((diffX), 2) + Math.Pow((diffY), 2));

            // Determine the 'calculaty' value
            double calculaty = velocity.X / diffX;

            // Calculate speed
            double speed = distance * calculaty;
            return speed;
        }
    }
}
