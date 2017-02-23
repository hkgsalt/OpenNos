﻿/*
 * This file is part of the OpenNos Emulator Project. See AUTHORS file for Copyright information
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 */

using OpenNos.Core;
using OpenNos.Data;
using OpenNos.Domain;
using OpenNos.GameObject.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenNos.GameObject
{
    public class BoxItem : Item
    {
        #region Instantiation

        public BoxItem(ItemDTO item) : base(item)
        {
        }

        #endregion

        #region Methods

        public override void Use(ClientSession session, ref ItemInstance inv, bool delay = false, string[] packetsplit = null)
        {
            switch (Effect)
            {
                case 1:
                    if (!delay)
                    {
                        session.SendPacket($"qna #guri^300^8023^{inv.Slot} {Language.Instance.GetMessageFromKey("ASK_RELEASE_PET")}");
                    }
                    else
                    {
                        if (session.CurrentMapInstance == session.Character.Miniland)
                        {
                            Mate mate = new Mate(session.Character, (short)EffectValue, 1, MateType.Pet);
                            session.Character.Mates.Add(mate);
                            session.SendPacket(session.Character.GenerateSay(string.Format(Language.Instance.GetMessageFromKey("YOU_GET_PET"), mate.Name), 1));
                            session.SendPacket(UserInterfaceHelper.Instance.GenerateInfo(Language.Instance.GetMessageFromKey("PET_LEAVE_BEAD")));
                            session.Character.Inventory.RemoveItemAmountFromInventory(1, inv.Id);
                            session.SendPacket(UserInterfaceHelper.Instance.GeneratePClear());
                            session.SendPackets(session.Character.GenerateScP());
                            session.SendPackets(session.Character.GenerateScN());
                        }
                    }

                    break;

                case 69:
                    if (EffectValue == 1 || EffectValue == 2)
                    {
                        BoxInstance box = session.Character.Inventory.LoadBySlotAndType<BoxInstance>(inv.Slot, InventoryType.Equipment);
                        if (box != null)
                        {
                            if (box.HoldingVNum == 0)
                            {
                                session.SendPacket($"wopen 44 {inv.Slot}");
                            }
                            else
                            {
                                SpecialistInstance cp = box.DeepCopy() as SpecialistInstance;
                                cp.Id = Guid.NewGuid();
                                cp.ItemVNum = box.HoldingVNum;
                                List<ItemInstance> newInv = session.Character.Inventory.AddToInventory(cp as ItemInstance);
                                if (newInv.Any())
                                {
                                    short Slot = inv.Slot;
                                    if (Slot != -1)
                                    {
                                        session.SendPacket(session.Character.GenerateSay($"{Language.Instance.GetMessageFromKey("ITEM_ACQUIRED")}: {newInv.First().Item.Name} + {newInv.First().Upgrade}", 12));
                                        session.Character.Inventory.RemoveItemAmountFromInventory(1, box.Id);
                                    }
                                }
                                else
                                {
                                    session.SendPacket(UserInterfaceHelper.Instance.GenerateMsg(Language.Instance.GetMessageFromKey("NOT_ENOUGH_PLACE"), 0));
                                }
                            }
                        }
                    }
                    if (EffectValue == 3)
                    {
                        BoxInstance box = session.Character.Inventory.LoadBySlotAndType<BoxInstance>(inv.Slot, InventoryType.Equipment);
                        if (box != null)
                        {
                            if (box.HoldingVNum == 0)
                            {
                                session.SendPacket($"guri 26 0 {inv.Slot}");
                            }
                            else
                            {
                                ItemInstance cp = box.DeepCopy();
                                cp.Id = Guid.NewGuid();
                                cp.ItemVNum = box.HoldingVNum;
                                List<ItemInstance> newInv = session.Character.Inventory.AddToInventory(cp);
                                if (newInv.Any())
                                {
                                    WearableInstance fairy = session.Character.Inventory.LoadBySlotAndType<WearableInstance>(newInv.First().Slot, newInv.First().Type);

                                    if (fairy != null)
                                    {
                                        fairy.ElementRate = box.ElementRate;
                                    }

                                    short Slot = inv.Slot;
                                    if (Slot != -1)
                                    {
                                        if (fairy != null)
                                        {
                                            session.SendPacket(session.Character.GenerateSay($"{Language.Instance.GetMessageFromKey("ITEM_ACQUIRED")}: {fairy.Item.Name} ({fairy.ElementRate}%)", 12));
                                        }
                                        session.Character.Inventory.RemoveItemAmountFromInventory(1, box.Id);
                                    }
                                }
                                else
                                {
                                    session.SendPacket(UserInterfaceHelper.Instance.GenerateMsg(Language.Instance.GetMessageFromKey("NOT_ENOUGH_PLACE"), 0));
                                }
                            }
                        }
                    }
                    if (EffectValue == 4)
                    {
                        BoxInstance box = session.Character.Inventory.LoadBySlotAndType<BoxInstance>(inv.Slot, InventoryType.Equipment);
                        if (box != null)
                        {
                            if (box.HoldingVNum == 0)
                            {
                                session.SendPacket($"guri 24 0 {inv.Slot}");
                            }
                            else
                            {
                                ItemInstance cp = box.DeepCopy();
                                cp.Id = Guid.NewGuid();
                                cp.ItemVNum = box.HoldingVNum;
                                List<ItemInstance> newInv = session.Character.Inventory.AddToInventory(cp);
                                if (newInv.Any())
                                {
                                    short Slot = inv.Slot;
                                    if (Slot != -1)
                                    {
                                        session.SendPacket(session.Character.GenerateSay($"{Language.Instance.GetMessageFromKey("ITEM_ACQUIRED")}: {newInv.First().Item.Name} x 1)", 12));
                                        session.Character.Inventory.RemoveItemAmountFromInventory(1, box.Id);
                                    }
                                }
                                else
                                {
                                    session.SendPacket(UserInterfaceHelper.Instance.GenerateMsg(Language.Instance.GetMessageFromKey("NOT_ENOUGH_PLACE"), 0));
                                }
                            }
                        }
                    }

                    break;

                default:
                    Logger.Log.Warn(string.Format(Language.Instance.GetMessageFromKey("NO_HANDLER_ITEM"), GetType()));
                    break;
            }
        }

        #endregion
    }
}