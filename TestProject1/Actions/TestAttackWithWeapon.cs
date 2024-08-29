using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSCSRPG.Models.Actions;
using SOSCSRPG.Services.Factories;
using Engine.Models;
using NUnit.Framework;

namespace TestSOSCSRPG.Models.Actions
{
    public class TestAttackWithWeapon
    {
        [Test]
        public void Test_Constructor_GoodParameters()
        {
            GameItem pointyStick = ItemFactory.CreateGameItem(1001);
            AttackWithWeapon attackWithWeapon = new AttackWithWeapon(pointyStick, "1d5");
            Assert.IsNotNull(attackWithWeapon);
        }
        [Test]
        public void Test_Constructor_ItemIsNotAWeapon()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                GameItem granolaBar = ItemFactory.CreateGameItem(2001);
                AttackWithWeapon attackWithWeapon = new AttackWithWeapon(granolaBar, "1d5");
            });
        }
        [Test]
        public void Test_Constructor_DamageDiceStringEmpty()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                GameItem pointyStick = ItemFactory.CreateGameItem(1001);
                AttackWithWeapon attackWithWeapon = new AttackWithWeapon(pointyStick, string.Empty);
            });
        }
    }
}
