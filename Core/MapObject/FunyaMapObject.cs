using System;
using System.Collections.Generic;
using MifuminSoft.funyak.Collision;
using MifuminSoft.funyak.Input;

namespace MifuminSoft.funyak.MapObject
{
    /// <summary>
    /// 主人公のマップオブジェクト
    /// </summary>
    public partial class FunyaMapObject : MapObjectBase, IUpdatableMapObject, IBounds
    {
        #region 主人公の状態

        /// <summary>
        /// 状態
        /// </summary>
        public FunyaMapObjectState State
        {
            get => state;
            set
            {
                if (value == state) return;
                StateCounter = 0;
                switch (value)
                {
                    case FunyaMapObjectState.Stand:
                        detectGravity = DetectGravityNormal;
                        updateSelfPreprocess = PreprocessNotFloating;
                        updateSelfMainProcess = MainProcessStanding;
                        break;
                    case FunyaMapObjectState.Float:
                        detectGravity = DetectGravityFloating;
                        updateSelfPreprocess = PreprocessFloating;
                        updateSelfMainProcess = MainProcessFloating;
                        break;
                    case FunyaMapObjectState.Fall:
                        detectGravity = DetectGravityNormal;
                        updateSelfPreprocess = PreprocessNotFloating;
                        updateSelfMainProcess = MainProcessFalling;
                        break;
                    case FunyaMapObjectState.Run:
                        detectGravity = DetectGravityNormal;
                        updateSelfPreprocess = PreprocessNotFloating;
                        updateSelfMainProcess = MainProcessRunning;
                        break;
                    case FunyaMapObjectState.Walk:
                        detectGravity = DetectGravityNormal;
                        updateSelfPreprocess = PreprocessNotFloating;
                        updateSelfMainProcess = MainProcessWalking;
                        break;
                    case FunyaMapObjectState.Charge:
                        detectGravity = DetectGravityNormal;
                        updateSelfPreprocess = PreprocessNotFloating;
                        updateSelfMainProcess = MainProcessCharging;
                        ChargeTime = 0;
                        break;
                    case FunyaMapObjectState.Jump:
                        detectGravity = DetectGravityNormal;
                        updateSelfPreprocess = PreprocessNotFloating;
                        updateSelfMainProcess = MainProcessJumping;
                        break;
                    case FunyaMapObjectState.Die:
                        detectGravity = b => { };
                        updateSelfPreprocess = () => { };
                        updateSelfMainProcess = (gravity, wind) => { };
                        break;
                    default:
                        throw new Exception("FunyaMapObjectのStateがおかしいぞ。");
                }
                state = value;
            }
        }

        private FunyaMapObjectState state;
        private Action<bool> detectGravity = null!; // Stateプロパティ初期化時に設定されるので実際は非null保証
        private Action updateSelfPreprocess = null!; // Stateプロパティ初期化時に設定されるので実際は非null保証
        private Action<double, double> updateSelfMainProcess = null!; // Stateプロパティ初期化時に設定されるので実際は非null保証

        public int StateCounter { get; set; }
        public int ChargeTime { get; set; }

        public double CollidedGravity { get; set; } = double.NaN;
        public double CollidedWind { get; set; } = double.NaN;

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
        public override double X { get; set; }

        /// <summary>
        /// Y座標
        /// </summary>
        public override double Y { get; set; }

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
        /// 地面の法線のX成分
        /// </summary>
        public double GroundNormalX { get; set; }

        /// <summary>
        /// 地面の法線のY成分
        /// </summary>
        public double GroundNormalY { get; set; }

        /// <summary>
        /// 地面の摩擦力
        /// </summary>
        public double GroundFriction { get; set; }

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

        public double Left => GetLeft(X);

        public double GetLeft(double x) => x - Size / 2;

        public double Top => GetTop(Y);

        public double GetTop(double y) => State switch
        {
            FunyaMapObjectState.Walk => y,
            FunyaMapObjectState.Charge => y,
            _ => y - Size / 2,
        };

        public double Right => GetRight(X);

        public double GetRight(double x) => x + Size / 2;

        public double Bottom => GetBottom(Y);

        public double GetBottom(double y) => y + Size / 2;

        public double GetCenterX(double x) => x;

        public double GetCenterY(double y) => State switch
        {
            FunyaMapObjectState.Walk => y + Size / 4,
            FunyaMapObjectState.Charge => y + Size / 4,
            _ => y,
        };

        /// <summary>
        /// 左側が壁と接触しているかどうか
        /// </summary>
        public bool TouchedLeft { get; set; }

        /// <summary>
        /// 上側が壁と接触しているかどうか
        /// </summary>
        public bool TouchedTop { get; set; }

        /// <summary>
        /// 右側が壁と接触しているかどうか
        /// </summary>
        public bool TouchedRight { get; set; }

        /// <summary>
        /// 下側が壁と接触しているかどうか
        /// </summary>
        public bool TouchedBottom { get; set; }

        #endregion

        #region パラメータ

        /// <summary>
        /// 外見を示す値
        /// </summary>
        public int Appearance = 0;

        /// <summary>
        /// 大きさ
        /// </summary>
        public double Size = 28.0;

        /// <summary>
        /// 走行速度
        /// </summary>
        public double RunSpeed = 200.0 / 60.0;

        /// <summary>
        /// 歩行速度
        /// </summary>
        public double WalkSpeed = 40.0 / 60.0;

        /// <summary>
        /// 走行加速度
        /// </summary>
        public double RunAccel = 400.0 / 60.0 / 60.0;

        /// <summary>
        /// 重力加速度
        /// </summary>
        public double GravityAccel = 300.0 / 60.0 / 60.0;

        /// <summary>
        /// 落下摩擦係数
        /// </summary>
        public double FallFriction = 300.0 / 40.0 / 60.0;

        /// <summary>
        /// 浮遊加速度
        /// </summary>
        public double FloatingAccel = 300.0 / 60.0 / 60.0;

        /// <summary>
        /// 浮遊摩擦係数
        /// </summary>
        public double FloatingFriction = 300.0 / 200.0 / 60.0;

        /// <summary>
        /// 回転加速度
        /// </summary>
        public double AngularAccel = 0.234375;

        /// <summary>
        /// 回転摩擦係数
        /// </summary>
        public double AngularFriction = 0.00667;

        /// <summary>
        /// 速度上限値
        /// </summary>
        public double VelocityLimit = 13;

        /// <summary>
        /// ジャンプの溜めデータ
        /// </summary>
        public IList<FunyaMapObjectCharge> JumpCharge = new List<FunyaMapObjectCharge>()
        {
            new FunyaMapObjectCharge()
            {
                Time = 9,
                Velocity = 255.0 / 60.0,
            },
            new FunyaMapObjectCharge()
            {
                Time = 30,
                Velocity = 205.0 / 60.0,
            },
            new FunyaMapObjectCharge()
            {
                Time = 90,
                Velocity = 155.0 / 60.0,
            },
            new FunyaMapObjectCharge()
            {
                Time = 0,
                Velocity = 60.0 / 60.0,
            },
        };

        /// <summary>
        /// 当たり判定の位置補正の許容誤差
        /// これ未満の補正量の場合位置補正をしない。
        /// </summary>
        public double PositionAdjustLowerLimit = 0.05;

        #endregion

        /// <summary>
        /// 場所を指定して主人公のマップオブジェクトを初期化します。
        /// </summary>
        /// <param name="x">X座標</param>
        /// <param name="y">Y座標</param>
        public FunyaMapObject(double x, double y)
        {
            State = FunyaMapObjectState.Float;

            Input = NullInput.Instance;
            X = x;
            Y = y;
            VelocityX = 0;
            VelocityY = 0;
            GroundNormalX = 0.0;
            GroundNormalY = -1.0;
            GroundFriction = 1.0;

            centerCollider = new PointCollider(this)
            {
                Reactivities = RegionAttributeFlag.Gravity | RegionAttributeFlag.Wind,
                OnCollided = OnCenterCollided,
            };
        }

        public void UpdateSelf(UpdateMapObjectArgs args)
        {
            var env = args.GetEnvironment(X, Y);
            var gravity = double.IsNaN(CollidedGravity) ? env.Gravity : CollidedGravity;
            var wind = double.IsNaN(CollidedWind) ? env.Wind : CollidedWind;

            detectGravity(gravity > 0);
            updateSelfPreprocess();
            updateSelfMainProcess(gravity, wind);
            centerCollider.SetPoint(GetCenterX(X), GetCenterY(Y));
        }

        private void DetectGravityNormal(bool inGravity)
        {
            if (!inGravity)
            {
                State = FunyaMapObjectState.Float;
                Direction = Direction.Front;
            }
        }

        private void DetectGravityFloating(bool inGravity)
        {
            if (inGravity)
            {
                State = FunyaMapObjectState.Fall;
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

        /// <summary>
        /// 浮遊状態でないときの前処理を行います。
        /// </summary>
        private void PreprocessNotFloating()
        {
            ResetRotation();
            UpdatePreviousValue();
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
        /// 回転を初期状態に戻します。
        /// </summary>
        private void ResetRotation()
        {
            AngularVelocity = 0;
            Angle = 0;
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
        /// 変化前の値を保持します。
        /// </summary>
        private void UpdatePreviousValue()
        {
            PreviousX = X;
            PreviousY = Y;
            PreviousVelocityX = VelocityX;
            PreviousVelocityY = VelocityY;
        }

        private void MainProcessStanding(double gravity, double wind)
        {
            if (Input.IsPressed(Keys.Left) || Input.IsPressed(Keys.Right))
            {
                State = FunyaMapObjectState.Run;
                MainProcessRunning(gravity, wind);
                return;
            }
            else if (Input.IsPushed(Keys.Jump))
            {
                State = FunyaMapObjectState.Charge;
                MainProcessCharging(gravity, wind);
                return;
            }
            else if (Input.IsPushed(Keys.Down))
            {
                State = FunyaMapObjectState.Walk;
                MainProcessWalking(gravity, wind);
                return;
            }
            else if (Input.IsPressed(Keys.Up))
            {
                Direction = Direction.Front;
            }

            UpdatePositionOnGround(0.0, 0.5, wind);
        }

        private void MainProcessRunning(double gravity, double wind)
        {
            if (Input.IsPressed(Keys.Jump))
            {
                State = FunyaMapObjectState.Charge;
                MainProcessCharging(gravity, wind);
            }
            else if (Input.IsPressed(Keys.Down))
            {
                State = FunyaMapObjectState.Walk;
                MainProcessWalking(gravity, wind);
            }
            else if (Input.IsPressed(Keys.Left))
            {
                Direction = Direction.Left;
                UpdatePositionOnGround(-RunSpeed, 1.0, wind);
            }
            else if (Input.IsPressed(Keys.Right))
            {
                Direction = Direction.Right;
                UpdatePositionOnGround(RunSpeed, 1.0, wind);
            }
            else
            {
                State = FunyaMapObjectState.Stand;
                MainProcessStanding(gravity, wind);
            }
        }

        private void MainProcessWalking(double gravity, double wind)
        {
            if (Input.IsPushed(Keys.Jump))
            {
                State = FunyaMapObjectState.Charge;
                MainProcessCharging(gravity, wind);
            }
            else if (!Input.IsPressed(Keys.Down))
            {
                State = FunyaMapObjectState.Stand;
                MainProcessStanding(gravity, wind);
            }
            else if (Input.IsPressed(Keys.Left))
            {
                Direction = Direction.Left;
                UpdatePositionWalking(-WalkSpeed);
            }
            else if (Input.IsPressed(Keys.Right))
            {
                Direction = Direction.Right;
                UpdatePositionWalking(WalkSpeed);
            }
            else
            {
                Direction = Direction.Front;
                UpdatePositionWalking(0.0);
            }
        }

        private void MainProcessCharging(double gravity, double wind)
        {
            if (!Input.IsPressed(Keys.Jump))
            {
                State = FunyaMapObjectState.Jump;
                var v = 0.0;
                foreach (var c in JumpCharge)
                {
                    v = c.Velocity;
                    if (ChargeTime < c.Time) break;
                }
                VelocityY = -v;
                MainProcessJumping(gravity, wind);
                return;
            }
            else if (Input.IsPushed(Keys.Down) && !Input.IsPushed(Keys.Jump))
            {
                State = FunyaMapObjectState.Walk;
                MainProcessWalking(gravity, wind);
                return;
            }
            ChargeTime++;
        }

        private void MainProcessJumping(double gravity, double wind)
        {
            if (Input.IsPressed(Keys.Down))
            {
                State = FunyaMapObjectState.Fall;
                MainProcessFalling(gravity, wind);
            }
            else
            {
                var accelY = gravity * GravityAccel;
                if (VelocityY >= -accelY)
                {
                    State = FunyaMapObjectState.Fall;
                    MainProcessFalling(gravity, wind);
                    return;
                }
                VelocityY += accelY;
                UpdatePosition();
            }
        }

        private void MainProcessFalling(double gravity, double wind)
        {
            double accelX = (Input.IsPressed(Keys.Left) ? -1.0 : Input.IsPressed(Keys.Right) ? 1.0 : 0.0) * FloatingAccel;
            double accelY = 0;
            if (accelX < 0)
            {
                Direction = Direction.Left;
            }
            else if (accelX > 0)
            {
                Direction = Direction.Right;
            }
            UpdatePositionFalling(gravity, wind, accelX, accelY, VelocityY < 0 ^ Input.IsPressed(Keys.Down));
        }

        private void MainProcessFloating(double gravity, double wind)
        {
            double accelX = Input.X * FloatingAccel;
            double accelY = Input.Y * FloatingAccel;
            UpdatePositionFloating(wind, accelX, accelY, Input.IsPressed(Keys.Jump) ? 2 : 1);
        }

        private void UpdatePositionOnGround(double targetVelocity, double accelScale, double wind)
        {
            // 地面の方向
            var groundX = -GroundNormalY;
            var groundY = GroundNormalX;
            // 地面方向の速度
            var velocityH = VelocityX * groundX + VelocityY * groundY;
            // 地面方向の風
            var windH = wind * groundX;
            // 走る力
            var runAccel = RunAccel * GroundFriction * accelScale * groundX;
            // 静止摩擦力
            var stopFriction = runAccel * 2.0;
            // 空気抵抗
            var friction = (velocityH - windH) * FloatingFriction;
            // 動く力が静止摩擦を超えているか
            var overFriction = friction < -stopFriction || stopFriction < friction;
            // 加速度
            var accelH = 0.0;
            if (VelocityX < targetVelocity)
            {
                accelH = runAccel - friction;
            }
            else if (VelocityX > targetVelocity)
            {
                accelH = -runAccel - friction;
            }
            else
            {
                if (friction < -stopFriction)
                {
                    accelH = -stopFriction - friction;
                }
                else if (RunAccel < friction)
                {
                    accelH = stopFriction - friction;
                }
            }
            velocityH += accelH;
            if (!overFriction)
            {
                if ((velocityH < targetVelocity && targetVelocity < velocityH - accelH) || (velocityH - accelH < targetVelocity && targetVelocity < velocityH))
                {
                    velocityH = targetVelocity;
                }
            }
            if ((TouchedRight && velocityH > 0) || (TouchedLeft && velocityH < 0))
            {
                velocityH = 0;
            }
            VelocityX = velocityH * groundX;
            VelocityY = velocityH * groundY;
            UpdatePosition();
        }

        private void UpdatePositionWalking(double targetVelocity)
        {
            // 地面の方向
            var groundX = -GroundNormalY;
            var groundY = GroundNormalX;
            // 地面方向の速度
            var velocityH = targetVelocity;
            if ((TouchedRight && velocityH > 0) || (TouchedLeft && velocityH < 0))
            {
                velocityH = 0;
            }
            VelocityX = velocityH * groundX;
            VelocityY = velocityH * groundY;
            UpdatePosition();
        }

        private void UpdatePositionFalling(double gravity, double wind, double accelX = 0.0, double accelY = 0.0, bool fastFall = false)
        {
            double frictionX = (VelocityX - wind) * FloatingFriction;
            double frictionY = VelocityY * (fastFall ? FloatingFriction : FallFriction);
            VelocityX += accelX - frictionX;
            VelocityY += accelY - frictionY + gravity * GravityAccel;
            if ((TouchedRight && VelocityX > 0) || (TouchedLeft && VelocityX < 0))
            {
                VelocityX = 0;
            }
            if ((TouchedBottom && VelocityY > 0) || (TouchedTop && VelocityY < 0))
            {
                VelocityY = 0;
            }
            UpdatePosition();
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
    }
}
