﻿using System;
using System.Runtime.InteropServices;
using OverRay.Hook.Structs;
using OverRay.Hook.Types;
using OverRay.Hook.Utils;

namespace OverRay.Hook.Mod
{
    public class Hud
    {
        public Hud(GameManager manager)
        {
            Manager = manager;
            TextSize = 9;
        }

        private GameManager Manager { get; }

        public int TextSize { get; set; }

        private readonly IntPtr Coordinates = Memory.GetPointerAtOffset((IntPtr) 0x500560, 0x224, 0x310, 0x34, 0x0, 0x1ac);
        private readonly IntPtr Glm = Memory.GetPointerAtOffset((IntPtr) 0x500298, 0x234, 0x10, 0xC, 0xB0);
        private readonly IntPtr Speed = Memory.GetPointerAtOffset((IntPtr) 0x500298, 0x60, 0x7F0);

        private readonly string VersionText = $"OverRay:v{OtherUtils.GetVersion}".Yellow();

        private bool Display { get; set; } = true;

        // TODO: methods
        // Text box background
        //Vector3 vpos1 = new Vector3 { X=14, Y=78, Z=0 };
        //Vector3 vpos2 = new Vector3 { X=94, Y=95, Z=0 };
        //using (StructPtr pos1 = new StructPtr(vpos1), pos2 = new StructPtr(vpos2))
        //{
        //    OVAddParticle(110, pos1, pos2, blueSparkTexture, 190);
        //}

        // Text box icon
        //Vector3 rpos1 = new Vector3 { X=4, Y=80, Z=0 };
        //Vector3 rpos2 = new Vector3 { X=14, Y=92, Z=200 };
        //using (StructPtr pos1 = new StructPtr(rpos1), pos2 = new StructPtr(rpos2))
        //{
        //    OVAddParticle(125, pos1, pos2, rayIcon, 5);
        //}

        public void InitializeHud()
        {
            Manager.Text.Actions.Set("coordinates", DrawCoordinates);
            Manager.Text.Actions.Set("glm", DrawGlmCoordinates);
            Manager.Text.Actions.Set("speed", DrawSpeed);
            Manager.Text.Actions.Set("level", DrawLevelName);
            Manager.Text.Actions.Set("VersionText", DrawVersion);

            Manager.Engine.Actions.Set("glm", DrawGlmPoint);
            Manager.Engine.Actions.Set("target", DrawTarget);
        }

        public void Show() => Display = true;

        public void Hide() => Display = false;

        private void DrawCoordinates()
        {
            if (!Display) return;

            Vector3 coordinates = Marshal.PtrToStructure<Vector3>(Coordinates);
            string coordinatesString = "X".KeyValue(coordinates.X.D3()).NL() +
                                       "Y".KeyValue(coordinates.Y.D3()).NL() +
                                       "Z".KeyValue(coordinates.Z.D3());

            Manager.Text.CustomText(coordinatesString, TextSize, 5, 5);
        }

        private void DrawGlmCoordinates()
        {
            if (!Display) return;

            Vector3 glm = Marshal.PtrToStructure<Vector3>(Glm);
            string glmString = "GLM".Yellow() + ":X".KeyValue(glm.X.D3()).NL() +
                               "::::Y".KeyValue(glm.Y.D3()).NL() +
                               "::::Z".KeyValue(glm.Z.D3());

            Manager.Text.CustomText(glmString, TextSize, 280, 5);
        }

        private void DrawSpeed()
        {
            if (!Display) return;

            Vector3 speed = Marshal.PtrToStructure<Vector3>(Speed);
            string speedString = "Speed".Yellow().KeyValue(speed.Length.D3()).NL() +
                                 "::::X".KeyValue(speed.X.D3()).NL() +
                                 "::::Y".KeyValue(speed.Y.D3()).NL() +
                                 "::::Z".KeyValue(speed.Z.D3());

            Manager.Text.CustomText(speedString, TextSize, 620, 5);
        }

        private void DrawGlmPoint()
        {
            if (!Display) return;

            Manager.Graphics.VCreatePart.Call(24576, Glm, 0, 2, 2, 2, TexturePointers.sparkTexture);
        }

        private void DrawLevelName()
        {
            if (!Display) return;

            Manager.Text.CustomText("Level".KeyValue(Manager.Engine.GetCurrentLevelName.Call()), TextSize, 5, 80);
        }

        private void DrawVersion()
        {
            Manager.Text.CustomText(VersionText, TextSize, 5, 970);
        }

        private void DrawTarget()
        {
            IntPtr raymanPtr = Marshal.ReadIntPtr((IntPtr)0x500578);
            SuperObject raymanSPO = Marshal.PtrToStructure<SuperObject>(raymanPtr);
            EngineObject engineObject = Marshal.PtrToStructure<EngineObject>(raymanSPO.engineObjectPtr);
            int stdGame = (int)engineObject.stdGamePtr;

            int[] interp = {
                0x00000071, 0x03020000, // Func_CibleLaPlusProcheavecAngles(, 1, 1, 10, Func_GetPersoSighting(), 160f, 40f);
                0x00000020, 0x0C030000, // 32,
                0x00000014, 0x0C030000, // 20,
                0x00000002, 0x0C030000, // 2,
                0x00000001, 0x0C030000, // 1,
                0x000000A3, 0x03030000, // Func_GetPersoSighting(),
                0x420C0000, 0x0D030000, // 35f
                0x420C0000, 0x0D030000, // 35f
                0x00000000, 0x00010000,
            };

            // TODO: use ArrayPtr()

            IntPtr interpArray = Marshal.AllocHGlobal(interp.Length * 4);
            for (int i = 0; i < interp.Length; i++) {
                Marshal.WriteInt32(interpArray, i * 4, interp[i]);
            }

            IntPtr paramArray = Marshal.AllocHGlobal(0x20 * 4);

            IntPtr interpPtrStart = interpArray + 0x8; // we start at the second node of the interpreter tree

            Manager.Engine.Code4PersoLePlusProche.Call((int)raymanPtr, (int)interpPtrStart, (int)paramArray);

            int result = Marshal.ReadInt32(paramArray, 0);
            if (result != 0) {

                IntPtr resultPtr = (IntPtr)result;

                EngineObject targetEngineObject = Marshal.PtrToStructure<EngineObject>(resultPtr);
                StandardGame targetStdGame = Marshal.PtrToStructure<StandardGame>(targetEngineObject.stdGamePtr);
                SuperObject targetSpo = Marshal.PtrToStructure<SuperObject>(targetStdGame.superObjectPtr);
                Matrix targetSpoMatrix = Marshal.PtrToStructure<Matrix>(targetSpo.matrixPtr);

                Vector3 targetPos = new Vector3 { X = targetSpoMatrix.x, Y = targetSpoMatrix.y, Z = targetSpoMatrix.z };
                using (StructPtr targetPosPtr = new StructPtr(targetPos))
                {
                    Manager.Graphics.VCreatePart.Call(24576, targetPosPtr, 0, 2, 2, 2, TexturePointers.blueSparkTexture);
                }
            }

            Marshal.FreeHGlobal(interpArray);
            Marshal.FreeHGlobal(paramArray);
        }
    }
}