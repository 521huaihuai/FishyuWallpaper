using FishyuAnimation;
using System;
using System.Windows.Forms;
using FishyuAnimation.Interpolation;
using FishyuAnimation.Animations;

namespace Wallpaper
{
    public partial class Form2 : Form, IAnimation
    {
        public Animation animation = new SimpleAnimation();
        private int index;
        public Form2()
        {
            InitializeComponent();
            //BackColor = Color.White;
            ShowInTaskbar = false;
            //TransparencyKey = Color.White;
            Opacity = 0.1;
            animation.iAnimalionInterface = this;
            animation.AnimalionTime = 3 * 1000;
            animation.OnAnimationStartEvent += Animation_OnAnimationStartEvent;
        }

        private void Animation_OnAnimationStartEvent()
        {

        }

        public void AnimalionRender(int animationIndex, InterpolationValue animationFrameInterpolation, InterpolationValue animationInterpolation)
        {
            index = animationIndex;
            this.Invoke((EventHandler)delegate
            {
                Opacity += 0.05;
            });
        }

        public void FrameAnimationFinished()
        {
        }

        public void PrepareAnimalion()
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (OnStateChangeListenerEvent != null)
            {
                OnStateChangeListenerEvent();
            }
        }

        public event OnStopListener OnStopListenerEvent;
        public delegate void OnStopListener();

        public event OnStateChangeListener OnStateChangeListenerEvent;
        public delegate void OnStateChangeListener();

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (OnStopListenerEvent != null)
            {
                OnStopListenerEvent();
            }
        }

        private void Form2_MouseLeave(object sender, EventArgs e)
        {
            animation.StopAnimation();
            Opacity = 0.1;
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            if (animation.AnimationState != Animation.AnimationStates.AnimationRuning)
            {
                animation.StartAnimalion();
            }
        }


        private void pictureBox2_MouseEnter(object sender, EventArgs e)
        {
            if (animation.AnimationState != Animation.AnimationStates.AnimationRuning)
            {
                animation.StartAnimalion();
            }
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            animation.StopAnimation();
            Opacity = 0.1;
        }

        private void pictureBox2_MouseLeave_1(object sender, EventArgs e)
        {
            animation.StopAnimation();
            Opacity = 0.1;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            ConfigureForm form = new ConfigureForm();
            form.ShowDialog();
        }

        private void pictureBox3_MouseEnter(object sender, EventArgs e)
        {
            if (animation.AnimationState != Animation.AnimationStates.AnimationRuning)
            {
                animation.StartAnimalion();
            }
        }

        private void pictureBox3_MouseLeave(object sender, EventArgs e)
        {
            animation.StopAnimation();
            Opacity = 0.1;
        }
    }
}
