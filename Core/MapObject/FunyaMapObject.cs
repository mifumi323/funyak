using System;
using System.Collections.Generic;
using MifuminSoft.funyak.Collision;
using MifuminSoft.funyak.Geometry;
using MifuminSoft.funyak.Input;
using MifuminSoft.funyak.MapEnvironment;

namespace MifuminSoft.funyak.MapObject
{

    /// <summary>
    /// 主人公のマップオブジェクト
    /// </summary>
    public class FunyaMapObject : MapObjectBase, IUpdatableMapObject, IBounds
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

        #region 当たり判定

        private readonly PointCollider centerCollider;
        public RegionAttributeFlag CenterReactivities
        {
            get => centerCollider.Reactivities;
            set => centerCollider.Reactivities = value;
        }

        private double c2rX;
        private double c2rY;
        private double c2rVelocityX;
        private double c2rVelocityY;
        private double c2rGroundNormalX;
        private double c2rGroundNormalY;
        private double c2rGroundFriction;
        private bool c2rTouchedLeft;
        private bool c2rTouchedTop;
        private bool c2rTouchedRight;
        private bool c2rTouchedBottom;

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

        public override void OnJoin(Map map, ColliderCollection colliderCollection)
        {
            colliderCollection.Add(centerCollider);
        }

        public override void OnLeave(Map map, ColliderCollection colliderCollection)
        {
            colliderCollection.Remove(centerCollider);
        }

        public void UpdateSelf(UpdateMapObjectArgs args)
        {
            var env = args.GetEnvironment(X, Y);
            var gravity = double.IsNaN(CollidedGravity) ? env.Gravity : CollidedGravity;
            var wind = env.Wind;

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

        public override void CheckCollision(CheckMapObjectCollisionArgs args)
        {
            // 共通当たり判定の位置更新
            centerCollider.SetPoint(X, Y);
            CollidedGravity = double.NaN;

            // 位置とか
            var x = X;
            var y = Y;
            var vx = VelocityX;
            var vy = VelocityY;
            var nx = GroundNormalX;
            var ny = GroundNormalY;
            var centerX = GetCenterX(x);
            var centerY = GetCenterY(y);
            var top = GetTop(y);
            var bottom = GetBottom(y);
            var left = GetLeft(x);
            var right = GetRight(x);

            // 暫定結果とか
            var friction = GroundFriction;
            var touchedLeft = false;
            var touchedTop = false;
            var touchedRight = false;
            var touchedBottom = false;

            // 当たり判定用図形とか
            var topSegment = new Segment2D(centerX, centerY - vy, centerX, top);
            var bottomSegment = new Segment2D(centerX, centerY - vy, centerX, bottom);
            var leftSegment = new Segment2D(centerX, centerY, left, centerY);
            var rightSegment = new Segment2D(centerX, centerY, right, centerY);
            var topVector = new Vector2D(centerX - x, top - y);
            var bottomVector = new Vector2D(centerX - x, bottom - y);
            var leftVector = new Vector2D(left - x, centerY - y);
            var rightVector = new Vector2D(right - x, centerY - y);
            var velocity = new Vector2D(vx, vy);

            // 位置調整オブジェクト
            var adjuster = new PositionAdjusterAverage();
            var adjusterHigh = new PositionAdjusterHigh();
            var adjusterLow = new PositionAdjusterLow();
            var adjusterLeft = new PositionAdjusterLeft();
            var adjusterRight = new PositionAdjusterRight();

            // 判定対象の種類ごとに振り分ける
            var collidableSegments = new List<CollidableSegment>();
            foreach (var mapObject in args.GetMapObjects(this))
            {
                if (mapObject is LineMapObject lineMapObject)
                {
                    collidableSegments.Add(lineMapObject.ToCollidableSegment());
                }

                if (mapObject is TileGridMapObject tileMapObject)
                {
                    tileMapObject.AddCollidableSegmentsToList(collidableSegments, left, top, right, bottom);
                }
            }

            // 線との当たり判定
            foreach (var collidableSegment in collidableSegments)
            {
                var lineSegment = collidableSegment.Segment;
                var lineVector = lineSegment.End - lineSegment.Start;
                var lineNormal = new Vector2D(lineVector.Y, -lineVector.X);
                var lineFriction = collidableSegment.Friction;
                lineNormal.Norm();
                var lineNormalNegative = -lineNormal;

                // 主人公の下側と線の上側
                if (collidableSegment.HitUpper)
                {
                    if (lineNormal.Y != 0)
                    {
                        var n = lineNormal.Y < 0 ? lineNormal : lineNormalNegative;
                        var collided = CheckCollisionSegment(n, bottomSegment, lineSegment, bottomVector, lineVector, x, y, velocity, lineFriction, adjusterHigh);
                        if (collided) touchedBottom = true;
                    }
                }

                // 主人公の上側と線の下側
                if (collidableSegment.HitBelow)
                {
                    if (lineNormal.Y != 0)
                    {
                        var n = lineNormal.Y > 0 ? lineNormal : lineNormalNegative;
                        var collided = CheckCollisionSegment(n, topSegment, lineSegment, topVector, lineVector, x, y, velocity, lineFriction, adjusterLow);
                        if (collided) touchedTop = true;
                    }
                }

                // 主人公の右側と線の左側
                if (collidableSegment.HitLeft)
                {
                    if (lineNormal.X != 0)
                    {
                        var n = lineNormal.X < 0 ? lineNormal : lineNormalNegative;
                        var collided = CheckCollisionSegment(n, rightSegment, lineSegment, rightVector, lineVector, x, y, velocity, lineFriction, adjusterLeft);
                        if (collided) touchedRight = true;
                    }
                }

                // 主人公の左側と線の右側
                if (collidableSegment.HitRight)
                {
                    if (lineNormal.X != 0)
                    {
                        var n = lineNormal.X > 0 ? lineNormal : lineNormalNegative;
                        var collided = CheckCollisionSegment(n, leftSegment, lineSegment, leftVector, lineVector, x, y, velocity, lineFriction, adjusterRight);
                        if (collided) touchedLeft = true;
                    }
                }
            }

            adjuster.Add(adjusterHigh);
            adjuster.Add(adjusterLow);
            adjuster.Add(adjusterLeft);
            adjuster.Add(adjusterRight);
            if (adjuster.HasValue)
            {
                x = adjuster.X;
                y = adjuster.Y;
                vx = adjuster.VelocityX;
                vy = adjuster.VelocityY;
            }
            if (adjusterHigh.HasValue)
            {
                nx = adjusterHigh.NormalX;
                ny = adjusterHigh.NormalY;
                friction = adjusterHigh.Friction;
            }

            c2rX = x;
            c2rY = y;
            c2rVelocityX = vx;
            c2rVelocityY = vy;
            c2rGroundNormalX = nx;
            c2rGroundNormalY = ny;
            c2rGroundFriction = friction;
            c2rTouchedLeft = touchedLeft;
            c2rTouchedTop = touchedTop;
            c2rTouchedRight = touchedRight;
            c2rTouchedBottom = touchedBottom;
        }

        public void OnCenterCollided(ref RegionPointCollision collision)
        {
            if (collision.RegionInfo.Flags.Has(RegionAttributeFlag.Gravity))
            {
                CollidedGravity = collision.RegionInfo.Gravity;
            }
        }

        public override void RealizeCollision(RealizeCollisionArgs args)
        {
            if (Math.Abs(X - c2rX) >= PositionAdjustLowerLimit) X = c2rX;
            if (Math.Abs(Y - c2rY) >= PositionAdjustLowerLimit) Y = c2rY;
            VelocityX = c2rVelocityX;
            VelocityY = c2rVelocityY;
            GroundNormalX = c2rGroundNormalX;
            GroundNormalY = c2rGroundNormalY;
            GroundFriction = c2rGroundFriction;
            TouchedLeft = c2rTouchedLeft;
            TouchedTop = c2rTouchedTop;
            TouchedRight = c2rTouchedRight;
            TouchedBottom = c2rTouchedBottom;

            if (Right < 0 || args.MapWidth < Left || Bottom < 0 || args.MapHeight < Top)
            {
                State = FunyaMapObjectState.Die;
            }

            RealizeCollision(c2rTouchedBottom);

            StateCounter++;
        }

        /// <summary>
        /// 衝突結果を反映
        /// </summary>
        private void RealizeCollision(bool landed)
        {
            switch (State)
            {
                case FunyaMapObjectState.Stand:
                case FunyaMapObjectState.Run:
                case FunyaMapObjectState.Walk:
                case FunyaMapObjectState.Charge:
                    if (!landed)
                    {
                        State = FunyaMapObjectState.Fall;
                    }
                    break;
                case FunyaMapObjectState.Jump:
                case FunyaMapObjectState.Fall:
                    if (landed)
                    {
                        if (Input.IsPressed(Keys.Left) || Input.IsPressed(Keys.Right))
                        {
                            State = FunyaMapObjectState.Run;
                        }
                        else if (Input.IsPressed(Keys.Jump))
                        {
                            State = FunyaMapObjectState.Charge;
                        }
                        else if (Input.IsPressed(Keys.Down))
                        {
                            State = FunyaMapObjectState.Walk;
                        }
                        else
                        {
                            State = FunyaMapObjectState.Stand;
                        }
                    }
                    break;
                case FunyaMapObjectState.Float:
                case FunyaMapObjectState.BreatheIn:
                case FunyaMapObjectState.BreatheOut:
                case FunyaMapObjectState.Tired:
                case FunyaMapObjectState.Frozen:
                case FunyaMapObjectState.Damaged:
                case FunyaMapObjectState.Die:
                    break;
                default:
                    throw new Exception("FunyaMapObjectのStateがおかしいぞ。");
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
        private bool CheckCollisionSegment(Vector2D lineNormal, Segment2D charaSegment, Segment2D lineSegment, Vector2D charaVector, Vector2D lineVector, double x, double y, Vector2D velocity, double friction, IPositionAdjuster adjuster)
        {
            if (velocity.Dot(lineNormal) <= Segment2D.DELTA)
            {
                if (charaSegment.IsCrossed(lineSegment))
                {
                    var lineStartToCharaEnd = charaSegment.End - lineSegment.Start;
                    var newPoint = lineSegment.Start + lineVector * (lineVector.Dot(lineStartToCharaEnd) / lineVector.LengthSq) - charaVector;
                    var newVelocity = velocity - lineNormal * velocity.Dot(lineNormal);
                    if (Math.Abs(newPoint.X - x) >= PositionAdjustLowerLimit ||
                        Math.Abs(newPoint.Y - y) >= PositionAdjustLowerLimit ||
                        Math.Abs(newVelocity.X - velocity.X) >= PositionAdjustLowerLimit ||
                        Math.Abs(newVelocity.Y - velocity.Y) >= PositionAdjustLowerLimit)
                    {
                        adjuster.Add(newPoint.X, newPoint.Y, newVelocity.X, newVelocity.Y, lineNormal.X, lineNormal.Y, friction);
                    }

                    return true;
                }
            }
            return false;
        }
    }
}
