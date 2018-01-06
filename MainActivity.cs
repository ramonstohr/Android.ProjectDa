using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views.InputMethods;
using Android.Widget;
using System;
using System.Collections.Generic;
using static Android.Views.View;

namespace Android1
{
    [Activity(Label = "Android1", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private Button button;
        private TextView text;
        private EditText inputText;
        private ImageView image;
        private int questionNr = 0;
        private List<Question> questions = new List<Question>();
        private InputMethodManager imm;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            button = FindViewById<Button>(Resource.Id.button1);
            text = FindViewById<TextView>(Resource.Id.textView1);
            image = FindViewById<ImageView>(Resource.Id.imageView1);
            inputText = FindViewById<EditText>(Resource.Id.editText1);
            text.TextAlignment = Android.Views.TextAlignment.Center;
            button.Click += Button_Click;
            questionNr = Processor.GetCurrentQuestion();
            questions = Processor.GetQuestions(Assets.Open("Fragen.xml"));
            if (questionNr >= questions.Count)
            {
                Toast.MakeText(this.BaseContext, "Du hast schon alles richtig beantwortet!", ToastLength.Long).Show();
                SetFinal();
            }
            else
            {
                using (System.IO.Stream istr = Assets.Open(questions[questionNr].URL))
                {
                    image.Measure(MeasureSpec.MakeMeasureSpec(0, Android.Views.MeasureSpecMode.Unspecified), MeasureSpec.MakeMeasureSpec(0, Android.Views.MeasureSpecMode.Unspecified));

                    Bitmap bit = BitmapFactory.DecodeStream(istr);
                    Bitmap xx = Bitmap.CreateScaledBitmap(bit, image.MeasuredWidth, image.MeasuredHeight, false);
                    image.SetImageBitmap(bit);
                }
                text.Text = questions[questionNr].QuestionText;
                imm = (InputMethodManager)GetSystemService(Android.Content.Context.InputMethodService);
            }
        }

        private void SetFinal()
        {
            text.Text = "";
            inputText.Visibility = Android.Views.ViewStates.Invisible;

            using (System.IO.Stream istr = Assets.Open("Ende.jpg"))
            {
                image.Measure(MeasureSpec.MakeMeasureSpec(0, Android.Views.MeasureSpecMode.Unspecified), MeasureSpec.MakeMeasureSpec(0, Android.Views.MeasureSpecMode.Unspecified));

                Bitmap bit = BitmapFactory.DecodeStream(istr);
                Bitmap xx = Bitmap.CreateScaledBitmap(bit, image.MeasuredWidth, image.MeasuredHeight, false);
                image.SetImageBitmap(bit);
            }
            button.Click -= Button_Click;
            button.Text = "Neustarten?";
            button.Click += Button_Reset_Click;
        }

        private void Button_Reset_Click(object sender, EventArgs e)
        {
            Processor.SaveQuestionNumber(0);
            Intent i = BaseContext.PackageManager.GetLaunchIntentForPackage(BaseContext.PackageName);
            i.AddFlags(ActivityFlags.ClearTop);

            StartActivity(i);
        }

        private void Button_Click(object sender, System.EventArgs e)
        {
            if (inputText.Text.ToLower().Equals(questions[questionNr].Answer.ToLower()))
            {
                questionNr++;
                imm.HideSoftInputFromWindow(inputText.WindowToken, 0);
                Processor.SaveQuestionNumber(questionNr);
                if (questionNr < questions.Count)
                {
                    inputText.Text = "";

                    Toast.MakeText(this.BaseContext, "Richtig", ToastLength.Long).Show();

                    text.Text = questions[questionNr].QuestionText;
                    using (System.IO.Stream istr = Assets.Open(questions[questionNr].URL))
                    {
                        image.Measure(MeasureSpec.MakeMeasureSpec(0, Android.Views.MeasureSpecMode.Unspecified), MeasureSpec.MakeMeasureSpec(0, Android.Views.MeasureSpecMode.Unspecified));

                        Bitmap bit = BitmapFactory.DecodeStream(istr);
                        image.SetImageBitmap(bit);
                    }
                    //System.IO.Stream istr = Assets.Open(questions[questionNr].URL);
                }
                else
                {
                    Toast.MakeText(this.BaseContext, "Alles Richtig", ToastLength.Long).Show();
                    SetFinal();
                }
            }
        }
    }
}