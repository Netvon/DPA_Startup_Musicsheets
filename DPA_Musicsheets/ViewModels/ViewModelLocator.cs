using Core.Editor;
using DPA_Musicsheets.Managers;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace DPA_Musicsheets.ViewModels
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<IFileService>(() => new Editor.WPFFileService());
            SimpleIoc.Default.Register<IMessageService>(() => new Editor.WPFMessageService());

            SimpleIoc.Default.Register<FileHandler>();
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<LilypondViewModel>();
            SimpleIoc.Default.Register<StaffsViewModel>();
            SimpleIoc.Default.Register<MidiPlayerViewModel>();

            

        }

        public MainViewModel MainViewModel => ServiceLocator.Current.GetInstance<MainViewModel>();

        public LilypondViewModel LilypondViewModel => ServiceLocator.Current.GetInstance<LilypondViewModel>();

        public StaffsViewModel StaffsViewModel => ServiceLocator.Current.GetInstance<StaffsViewModel>();

        public MidiPlayerViewModel MidiPlayerViewModel => ServiceLocator.Current.GetInstance<MidiPlayerViewModel>();

        public static void Cleanup()
        {
            ServiceLocator.Current.GetInstance<MidiPlayerViewModel>().Cleanup();
            ServiceLocator.Current.GetInstance<LilypondViewModel>().Cleanup();
        }
    }
}