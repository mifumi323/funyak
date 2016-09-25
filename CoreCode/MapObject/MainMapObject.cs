using System;
using System.Collections.Generic;
using MifuminSoft.funyak.CollisionHelper;
using MifuminSoft.funyak.Input;
using MifuminSoft.funyak.MapEnvironment;

namespace MifuminSoft.funyak.MapObject
{
    /// <summary>
    /// 主人公のマップオブジェクトの状態
    /// </summary>
    public enum MainMapObjectState
    {
        Standing,
        Floating,
        Falling,
        Running,
    }

    /// <summary>
    /// 主人公のマップオブジェクト
    /// </summary>
    public class MainMapObject : IDynamicMapObject
    {
        #region 主人公の状態

        /// <summary>
        /// 状態
        /// </summary>
        public MainMapObjectState State
        {
            get
            {
                return state;
            }
            set
            {
                if (value == state) return;
                StateCounter = 0;
                switch (value)
                {
                    case MainMapObjectState.Standing:
                        detectGravity = DetectGravityNormal;
                        updateSelfPreprocess = PreprocessNotFloating;
                        updateSelfMainProcess = MainProcessStanding;
                        break;
                    case MainMapObjectState.Floating:
                        detectGravity = DetectGravityFloating;
                        updateSelfPreprocess = PreprocessFloating;
                        updateSelfMainProcess = MainProcessFloating;
                        break;
                    case MainMapObjectState.Falling:
                        detectGravity = DetectGravityNormal;
                        updateSelfPreprocess = PreprocessNotFloating;
                        updateSelfMainProcess = MainProcessFalling;
                        break;
                    case MainMapObjectState.Running:
                        detectGravity = DetectGravityNormal;
                        updateSelfPreprocess = PreprocessNotFloating;
                        updateSelfMainProcess = MainProcessRunning;
                        break;
                    default:
                        throw new Exception("MainMapObjectのStateがおかしいぞ。");
                }
                state = value;
            }
        }

        private MainMapObjectState state;
        private Action<bool> detectGravity;
        private Action updateSelfPreprocess;
        private Action<IMapEnvironment> updateSelfMainProcess;

        public int StateCounter { get; private set; }

        /// <summary>
        /// 方向
        /// </summary>
        public Direction Direction { get; set; }

        #endregion

        #region 主人公の位置

        /// <summary>
        /// 入力
        /// </summary>
        public IInput Input { get; set; }

        /// <summary>
        /// X座標
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Y座標
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// 速度のX成分
        /// </summary>
        public double VelocityX { get; set; }

        /// <summary>
        /// 速度のY成分
        /// </summary>
        public double VelocityY { get; set; }

        /// <summary>
        /// 角度
        /// </summary>
        public double Angle { get; set; }

        /// <summary>
        /// 角速度
        /// </summary>
        public double AngularVelocity { get; set; }

        /// <summary>
        /// 前フレームのX座標
        /// </summary>
        public double PreviousX { get; set; }

        /// <summary>
        /// 前フレームのY座標
        /// </summary>
        public double PreviousY { get; set; }

        /// <summary>
        /// 前フレームの速度のX成分
        /// </summary>
        public double PreviousVelocityX { get; set; }

        /// <summary>
        /// 前フレームの速度のY成分
        /// </summary>
        public double PreviousVelocityY { get; set; }

        public double Left
        {
            get
            {
                return GetLeft(X);
            }
        }

        public double GetLeft(double x)
        {
            return x - 14;
        }

        public double Top
        {
            get
            {
                return GetTop(Y);
            }
        }

        public double GetTop(double y)
        {
            return y - 14;
        }

        public double Right
        {
            get
            {
                return GetRight(X);
            }
        }

        public double GetRight(double x)
        {
            return x + 14;
        }

        public double Bottom
        {
            get
            {
                return GetBottom(Y);
            }
        }

        public double GetBottom(double y)
        {
            return y + 14;
        }

        #endregion

        #region パラメータ

        /// <summary>
        /// 外見を示す値
        /// </summary>
        public int Appearance { get; set; }

        /// <summary>
        /// 走行速度
        /// </summary>
        public double RunSpeed { get; set; } = 200.0 / 60.0;

        /// <summary>
        /// 走行加速度
        /// </summary>
        public double RunAccel { get; set; } = 400.0 / 60.0 / 60.0;

        /// <summary>
        /// 重力加速度
        /// </summary>
        public double GravityAccel { get; set; } = 0.4 / 3;

        /// <summary>
        /// 落下摩擦係数
        /// </summary>
        public double FallFriction { get; set; } = 0.1;

        /// <summary>
        /// 浮遊加速度
        /// </summary>
        public double FloatingAccel { get; set; } = 0.16;

        /// <summary>
        /// 浮遊摩擦係数
        /// </summary>
        public double FloatingFriction { get; set; } = 0.0173;

        /// <summary>
        /// 回転加速度
        /// </summary>
        public double AngularAccel { get; set; } = 0.234375;

        /// <summary>
        /// 回転摩擦係数
        /// </summary>
        public double AngularFriction { get; set; } = 0.00667;

        /// <summary>
        /// 速度上限値
        /// </summary>
        public double VelocityLimit { get; set; } = 13;

        #endregion

        class PositionAdjuster
        {
            private double x = 0.0;
            private double y = 0.0;
            private double vx = 0.0;
            private double vy = 0.0;
            private int count = 0;

            public void Add(double x, double y, double vx, double vy)
            {
                this.x += x;
                this.y += y;
                this.vx += vx;
                this.vy += vy;
                count++;
            }

            public bool HasValue => count > 0;
            public double X => x / count;
            public double Y => y / count;
            public double VelocityX => vx / count;
            public double VelocityY => vy / count;
        }

        /// <summary>
        /// 場所を指定して主人公のマップオブジェクトを初期化します。
        /// </summary>
        /// <param name="x">X座標</param>
        /// <param name="y">Y座標</param>
        public MainMapObject(double x, double y)
        {
            State = MainMapObjectState.Floating;

            Input = new NullInput();
            X = x;
            Y = y;
            VelocityX = 0;
            VelocityY = 0;
        }

        public void UpdateSelf(UpdateMapObjectArgs args)
        {
            var env = args.GetEnvironment(X, Y);

            detectGravity(env.Gravity > 0);
            updateSelfPreprocess();
            updateSelfMainProcess(env);
        }

        private void DetectGravityNormal(bool inGravity)
        {
            if (!inGravity)
            {
                State = MainMapObjectState.Floating;
                Direction = Direction.Front;
            }
        }

        private void DetectGravityFloating(bool inGravity)
        {
            if (inGravity)
            {
                State = MainMapObjectState.Falling;
                if (Input.IsPressed(Keys.Left))
                {
                    Direction = Direction.Left;
                }
                else if (Input.IsPressed(Keys.Right))
                {
                    Direction = Direction.Right;
                }
            }
        }

        private void MainProcessStanding(IMapEnvironment env)
        {
            if (Input.IsPressed(Keys.Left) || Input.IsPressed(Keys.Right))
            {
                State = MainMapObjectState.Running;
                MainProcessRunning(env);
                return;
            }

            UpdatePositionOnGround(0.0, 0.5, env.Wind);
        }

        private void UpdatePositionOnGround(double targetVelocity, double accelScale, double wind)
        {
            var runAccel = RunAccel * accelScale;
            var stopFriction = runAccel * 2.0;
            var friction = (VelocityX - wind) * FloatingFriction;
            var overFriction = friction < -stopFriction || stopFriction < friction;
            var accel = 0.0;
            if (VelocityX < targetVelocity)
            {
                accel = runAccel - friction;
            }
            else if (VelocityX > targetVelocity)
            {
                accel = -runAccel - friction;
            }
            else
            {
                if (friction < -stopFriction)
                {
                    accel = -stopFriction - friction;
                }
                else if (RunAccel < friction)
                {
                    accel = stopFriction - friction;
                }
            }
            VelocityX += accel;
            if (!overFriction)
            {
                if ((VelocityX < targetVelocity && targetVelocity < VelocityX - accel) || (VelocityX - accel < targetVelocity && targetVelocity < VelocityX))
                {
                    VelocityX = targetVelocity;
                }
            }
            UpdatePosition();
        }

        private void MainProcessFloating(IMapEnvironment env)
        {
            double accelX = Input.X * FloatingAccel;
            double accelY = Input.Y * FloatingAccel;
            UpdatePositionFloating(env.Wind, accelX, accelY, Input.IsPressed(Keys.Jump) ? 2 : 1);
        }

        private void MainProcessFalling(IMapEnvironment env)
        {
            double accelX = Input.X * FloatingAccel;
            double accelY = 0;
            if (accelX < 0)
            {
                Direction = Direction.Left;
            }
            else if (accelX > 0)
            {
                Direction = Direction.Right;
            }
            UpdatePositionFalling(env.Gravity, env.Wind, accelX, accelY);
        }

        private void MainProcessRunning(IMapEnvironment env)
        {
            if (Input.IsPressed(Keys.Left))
            {
                Direction = Direction.Left;
                UpdatePositionOnGround(-RunSpeed, 1.0, env.Wind);
            }
            else if (Input.IsPressed(Keys.Right))
            {
                Direction = Direction.Right;
                UpdatePositionOnGround(RunSpeed, 1.0, env.Wind);
            }
            else
            {
                State = MainMapObjectState.Standing;
                MainProcessStanding(env);
            }
        }

        /// <summary>
        /// 浮遊状態の前処理を行います。
        /// </summary>
        private void PreprocessFloating()
        {
            RotateSelf();
            UpdatePreviousValue();
        }

        /// <summary>
        /// 浮遊状態の位置更新を行います。
        /// </summary>
        /// <param name="wind">風速</param>
        /// <param name="accelX">X軸方向の加速度</param>
        /// <param name="accelY">Y軸方向の加速度</param>
        /// <param name="accelCount">加速・減速を行う回数</param>
        private void UpdatePositionFloating(double wind, double accelX = 0.0, double accelY = 0.0, int accelCount = 1)
        {
            for (int i = 0; i < accelCount; i++)
            {
                double frictionX = (VelocityX - wind) * FloatingFriction;
                double frictionY = VelocityY * FloatingFriction;
                VelocityX += accelX - frictionX;
                VelocityY += accelY - frictionY;
            }
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            if (VelocityX * VelocityX + VelocityY * VelocityY > VelocityLimit * VelocityLimit)
            {
                var r = VelocityLimit / Math.Sqrt(VelocityX * VelocityX + VelocityY * VelocityY);
                VelocityX *= r;
                VelocityY *= r;
            }
            X += VelocityX;
            Y += VelocityY;
        }

        /// <summary>
        /// 変化前の値を保持します。
        /// </summary>
        private void UpdatePreviousValue()
        {
            PreviousX = X;
            PreviousY = Y;
            PreviousVelocityX = VelocityX;
            PreviousVelocityY = VelocityY;
        }

        /// <summary>
        /// 自身の移動に応じて回転させます。
        /// </summary>
        private void RotateSelf()
        {
            var adx = X - PreviousX;
            var ady = Y - PreviousY;
            var addx = VelocityX - PreviousVelocityX;
            var addy = VelocityY - PreviousVelocityY;
            AngularVelocity += (adx * addy - ady * addx) * AngularAccel - AngularVelocity * AngularFriction;
            Angle += AngularVelocity;
            while (Angle > 180) Angle -= 360;
            while (Angle < -180) Angle += 360;
        }

        /// <summary>
        /// 浮遊状態でないときの前処理を行います。
        /// </summary>
        private void PreprocessNotFloating()
        {
            ResetRotation();
            UpdatePreviousValue();
        }

        private void UpdatePositionFalling(double gravity, double wind, double accelX = 0.0, double accelY = 0.0)
        {
            double frictionX = (VelocityX - wind) * FloatingFriction;
            double frictionY = VelocityY * FallFriction;
            VelocityX += accelX - frictionX;
            VelocityY += accelY - frictionY + gravity * GravityAccel;
            UpdatePosition();
        }

        /// <summary>
        /// 回転を初期状態に戻します。
        /// </summary>
        private void ResetRotation()
        {
            AngularVelocity = 0;
            Angle = 0;
        }

        public Action CheckCollision(CheckMapObjectCollisionArgs args)
        {

            // 種類ごとに振り分ける
            var lineMapObjects = new List<LineMapObject>();
            foreach (var mapObject in args.GetMapObjects(this))
            {
                var lineMapObject = mapObject as LineMapObject;
                if (lineMapObject != null)
                {
                    lineMapObjects.Add(lineMapObject);
                }
            }

            // 結果
            var tempX = X;
            var tempY = Y;
            var tempVX = VelocityX;
            var tempVY = VelocityY;
            var landed = false;

            // X軸移動とY軸移動が別々に起こったものとして2回判定を行う。
            // こうすることにより、すり抜けバグを防止できる。
            for (int i = 0; i < 2; i++)
            {
                if (i == 0)
                {
                    // 1回目はY軸移動前
                    tempY -= tempVY;
                }
                else
                {
                    // 2回目はY軸移動後
                    tempY += tempVY;
                }

                // 当たり判定用図形を生成
                var topSegment = new Segment2D(tempX, tempY, tempX, GetTop(tempY));
                var bottomSegment = new Segment2D(tempX, tempY, tempX, GetBottom(tempY));
                var leftSegment = new Segment2D(tempX, tempY, GetLeft(tempX), tempY);
                var rightSegment = new Segment2D(tempX, tempY, GetRight(tempX), tempY);
                var topVector = topSegment.End - topSegment.Start;
                var bottomVector = bottomSegment.End - bottomSegment.Start;
                var leftVector = leftSegment.End - leftSegment.Start;
                var rightVector = rightSegment.End - rightSegment.Start;
                var velocity = new Vector2D(tempVX, tempVY);

                var adjuster = new PositionAdjuster();

                // 線との当たり判定
                foreach (var lineMapObject in lineMapObjects)
                {
                    var lineSegment = lineMapObject.ToSegment2D();
                    var lineVector = lineSegment.End - lineSegment.Start;
                    var lineNormal = new Vector2D(lineVector.Y, -lineVector.X);
                    lineNormal.Norm();
                    var lineNormalNegative = -lineNormal;

                    // 主人公の下側と線の上側
                    if (lineMapObject.HitUpper)
                    {
                        if (lineNormal.Y != 0)
                        {
                            var n = lineNormal.Y < 0 ? lineNormal : lineNormalNegative;
                            var collided = CheckCollisionSegment(n, bottomSegment, lineSegment, bottomVector, lineVector, velocity, adjuster);
                            if (collided) landed = true;
                        }
                    }

                    // 主人公の上側と線の下側
                    if (lineMapObject.HitBelow)
                    {
                        if (lineNormal.Y != 0)
                        {
                            var n = lineNormal.Y > 0 ? lineNormal : lineNormalNegative;
                            var collided = CheckCollisionSegment(n, topSegment, lineSegment, topVector, lineVector, velocity, adjuster);
                        }
                    }

                    // 主人公の右側と線の左側
                    if (lineMapObject.HitLeft)
                    {
                        if (lineNormal.X != 0)
                        {
                            var n = lineNormal.X < 0 ? lineNormal : lineNormalNegative;
                            var collided = CheckCollisionSegment(n, rightSegment, lineSegment, rightVector, lineVector, velocity, adjuster);
                        }
                    }

                    // 主人公の左側と線の右側
                    if (lineMapObject.HitRight)
                    {
                        if (lineNormal.X != 0)
                        {
                            var n = lineNormal.X > 0 ? lineNormal : lineNormalNegative;
                            var collided = CheckCollisionSegment(n, leftSegment, lineSegment, leftVector, lineVector, velocity, adjuster);
                        }
                    }
                }

                if (adjuster.HasValue)
                {
                    tempX = adjuster.X;
                    tempY = adjuster.Y;
                    tempVX = adjuster.VelocityX;
                    tempVY = adjuster.VelocityY;
                }
            }

            return () =>
            {
                X = tempX;
                Y = tempY;
                VelocityX = tempVX;
                VelocityY = tempVY;

                RealizeCollision(landed);

                // TODO: 本来ここでやることじゃない
                StateCounter++;
            };
        }

        /// <summary>
        /// 衝突結果を反映
        /// </summary>
        private void RealizeCollision(bool landed)
        {
            switch (State)
            {
                case MainMapObjectState.Standing:
                case MainMapObjectState.Running:
                    if (!landed)
                    {
                        State = MainMapObjectState.Falling;
                    }
                    break;
                case MainMapObjectState.Floating:
                    break;
                case MainMapObjectState.Falling:
                    if (landed)
                    {
                        State = MainMapObjectState.Standing;
                    }
                    break;
                default:
                    throw new Exception("MainMapObjectのStateがおかしいぞ。");
            }
        }

        /// <summary>
        /// 線分との当たり判定
        /// </summary>
        /// <param name="lineNormal">線分の、当たり判定を行いたい方向に向けた法線ベクトル</param>
        /// <param name="charaSegment">キャラクターの、当たり判定に用いる線分</param>
        /// <param name="lineSegment">当たり判定対象の線分</param>
        /// <param name="charaVector">キャラクターの線分の、始点から終点へのベクトル</param>
        /// <param name="lineVector">当たり判定対象の線分の、始点から終点へのベクトル</param>
        /// <param name="velocity">当たり判定対象の線分に対する、キャラクターの相対速度</param>
        /// <param name="adjuster">位置調整オブジェクト</param>
        /// <returns>当たっていたらtrue</returns>
        private bool CheckCollisionSegment(Vector2D lineNormal, Segment2D charaSegment, Segment2D lineSegment, Vector2D charaVector, Vector2D lineVector, Vector2D velocity, PositionAdjuster adjuster)
        {
            if (velocity.Dot(lineNormal) <= 0)
            {
                if (Collision2D.SegmentSegment(charaSegment, lineSegment))
                {
                    var lineStartToCharaEnd = charaSegment.End - lineSegment.Start;
                    var newPoint = lineSegment.Start + lineVector * (lineVector.Dot(lineStartToCharaEnd) / lineVector.LengthSq) - charaVector;
                    var newVelocity = velocity - lineNormal * velocity.Dot(lineNormal);
                    adjuster.Add(newPoint.X, newPoint.Y, newVelocity.X, newVelocity.Y);

                    return true;
                }
            }
            return false;
        }
    }
}
