using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Linq;
using Crestron;
using Crestron.Logos.SplusLibrary;
using Crestron.Logos.SplusObjects;
using Crestron.SimplSharp;

namespace UserModule_GET_DAYOF_WEEK
{
    public class UserModuleClass_GET_DAYOF_WEEK : SplusObject
    {
        static CCriticalSection g_criticalSection = new CCriticalSection();
        
        Crestron.Logos.SplusObjects.DigitalInput GET;
        Crestron.Logos.SplusObjects.AnalogOutput DAYOFWEEKNUMBER;
        Crestron.Logos.SplusObjects.StringOutput DAYOFWEEK__DOLLAR__;
        ushort NUMDAYOFWEEK = 0;
        private void DAYOFWK (  SplusExecutionContext __context__ ) 
            { 
            
            __context__.SourceCodeLine = 26;
            NUMDAYOFWEEK = (ushort) ( Functions.GetDayOfWeekNum() ) ; 
            __context__.SourceCodeLine = 27;
            DAYOFWEEKNUMBER  .Value = (ushort) ( NUMDAYOFWEEK ) ; 
            
            }
            
        private ushort DAYOFWEEK (  SplusExecutionContext __context__ ) 
            { 
            
            __context__.SourceCodeLine = 31;
            
                {
                int __SPLS_TMPVAR__SWTCH_1__ = ((int)NUMDAYOFWEEK);
                
                    { 
                    if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_1__ == ( 0) ) ) ) 
                        { 
                        __context__.SourceCodeLine = 34;
                        DAYOFWEEK__DOLLAR__  .UpdateValue ( "Sunday"  ) ; 
                        } 
                    
                    else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_1__ == ( 1) ) ) ) 
                        { 
                        __context__.SourceCodeLine = 36;
                        DAYOFWEEK__DOLLAR__  .UpdateValue ( "Monday"  ) ; 
                        } 
                    
                    else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_1__ == ( 2) ) ) ) 
                        { 
                        __context__.SourceCodeLine = 38;
                        DAYOFWEEK__DOLLAR__  .UpdateValue ( "Tuesday"  ) ; 
                        } 
                    
                    else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_1__ == ( 3) ) ) ) 
                        { 
                        __context__.SourceCodeLine = 40;
                        DAYOFWEEK__DOLLAR__  .UpdateValue ( "Wednesday"  ) ; 
                        } 
                    
                    else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_1__ == ( 4) ) ) ) 
                        { 
                        __context__.SourceCodeLine = 42;
                        DAYOFWEEK__DOLLAR__  .UpdateValue ( "Thursday"  ) ; 
                        } 
                    
                    else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_1__ == ( 5) ) ) ) 
                        { 
                        __context__.SourceCodeLine = 44;
                        DAYOFWEEK__DOLLAR__  .UpdateValue ( "Friday"  ) ; 
                        } 
                    
                    else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_1__ == ( 6) ) ) ) 
                        { 
                        __context__.SourceCodeLine = 46;
                        DAYOFWEEK__DOLLAR__  .UpdateValue ( "Saturday"  ) ; 
                        } 
                    
                    } 
                    
                }
                
            
            
            return 0; // default return value (none specified in module)
            }
            
        object GET_OnPush_0 ( Object __EventInfo__ )
        
            { 
            Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
            try
            {
                SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
                
                __context__.SourceCodeLine = 52;
                DAYOFWEEK (  __context__  ) ; 
                __context__.SourceCodeLine = 53;
                DAYOFWK (  __context__  ) ; 
                
                
            }
            catch(Exception e) { ObjectCatchHandler(e); }
            finally { ObjectFinallyHandler( __SignalEventArg__ ); }
            return this;
            
        }
        
    public override object FunctionMain (  object __obj__ ) 
        { 
        try
        {
            SplusExecutionContext __context__ = SplusFunctionMainStartCode();
            
            __context__.SourceCodeLine = 60;
            NUMDAYOFWEEK = (ushort) ( Functions.GetDayOfWeekNum() ) ; 
            
            
        }
        catch(Exception e) { ObjectCatchHandler(e); }
        finally { ObjectFinallyHandler(); }
        return __obj__;
        }
        
    
    public override void LogosSplusInitialize()
    {
        SocketInfo __socketinfo__ = new SocketInfo( 1, this );
        InitialParametersClass.ResolveHostName = __socketinfo__.ResolveHostName;
        _SplusNVRAM = new SplusNVRAM( this );
        
        GET = new Crestron.Logos.SplusObjects.DigitalInput( GET__DigitalInput__, this );
        m_DigitalInputList.Add( GET__DigitalInput__, GET );
        
        DAYOFWEEKNUMBER = new Crestron.Logos.SplusObjects.AnalogOutput( DAYOFWEEKNUMBER__AnalogSerialOutput__, this );
        m_AnalogOutputList.Add( DAYOFWEEKNUMBER__AnalogSerialOutput__, DAYOFWEEKNUMBER );
        
        DAYOFWEEK__DOLLAR__ = new Crestron.Logos.SplusObjects.StringOutput( DAYOFWEEK__DOLLAR____AnalogSerialOutput__, this );
        m_StringOutputList.Add( DAYOFWEEK__DOLLAR____AnalogSerialOutput__, DAYOFWEEK__DOLLAR__ );
        
        
        GET.OnDigitalPush.Add( new InputChangeHandlerWrapper( GET_OnPush_0, false ) );
        
        _SplusNVRAM.PopulateCustomAttributeList( true );
        
        NVRAM = _SplusNVRAM;
        
    }
    
    public override void LogosSimplSharpInitialize()
    {
        
        
    }
    
    public UserModuleClass_GET_DAYOF_WEEK ( string InstanceName, string ReferenceID, Crestron.Logos.SplusObjects.CrestronStringEncoding nEncodingType ) : base( InstanceName, ReferenceID, nEncodingType ) {}
    
    
    
    
    const uint GET__DigitalInput__ = 0;
    const uint DAYOFWEEKNUMBER__AnalogSerialOutput__ = 0;
    const uint DAYOFWEEK__DOLLAR____AnalogSerialOutput__ = 1;
    
    [SplusStructAttribute(-1, true, false)]
    public class SplusNVRAM : SplusStructureBase
    {
    
        public SplusNVRAM( SplusObject __caller__ ) : base( __caller__ ) {}
        
        
    }
    
    SplusNVRAM _SplusNVRAM = null;
    
    public class __CEvent__ : CEvent
    {
        public __CEvent__() {}
        public void Close() { base.Close(); }
        public int Reset() { return base.Reset() ? 1 : 0; }
        public int Set() { return base.Set() ? 1 : 0; }
        public int Wait( int timeOutInMs ) { return base.Wait( timeOutInMs ) ? 1 : 0; }
    }
    public class __CMutex__ : CMutex
    {
        public __CMutex__() {}
        public void Close() { base.Close(); }
        public void ReleaseMutex() { base.ReleaseMutex(); }
        public int WaitForMutex() { return base.WaitForMutex() ? 1 : 0; }
    }
     public int IsNull( object obj ){ return (obj == null) ? 1 : 0; }
}


}
