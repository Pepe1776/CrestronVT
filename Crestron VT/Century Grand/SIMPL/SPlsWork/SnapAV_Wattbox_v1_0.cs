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
using UserModule_BASE64;

namespace UserModule_SNAPAV_WATTBOX_V1_0
{
    public class UserModuleClass_SNAPAV_WATTBOX_V1_0 : SplusObject
    {
        static CCriticalSection g_criticalSection = new CCriticalSection();
        
        Crestron.Logos.SplusObjects.DigitalInput RESET_ALL_OUTLETS;
        Crestron.Logos.SplusObjects.DigitalInput AUTO_REBOOT_ON;
        Crestron.Logos.SplusObjects.DigitalInput AUTO_REBOOT_OFF;
        Crestron.Logos.SplusObjects.DigitalInput GET_STATUS;
        InOutArray<Crestron.Logos.SplusObjects.DigitalInput> OUTLET_ON;
        InOutArray<Crestron.Logos.SplusObjects.DigitalInput> OUTLET_OFF;
        InOutArray<Crestron.Logos.SplusObjects.DigitalInput> OUTLET_RESET;
        InOutArray<Crestron.Logos.SplusObjects.DigitalOutput> OUTLET_STATUS;
        Crestron.Logos.SplusObjects.StringOutput HOST_NAME__DOLLAR__;
        Crestron.Logos.SplusObjects.StringOutput HARDWARE_VERSION__DOLLAR__;
        Crestron.Logos.SplusObjects.StringOutput SERIAL_NUMBER__DOLLAR__;
        Crestron.Logos.SplusObjects.StringOutput SAFE_VOLTAGE_STATUS__DOLLAR__;
        Crestron.Logos.SplusObjects.StringOutput VOLTAGE_VALUE__DOLLAR__;
        Crestron.Logos.SplusObjects.StringOutput CURRENT_VALUE__DOLLAR__;
        Crestron.Logos.SplusObjects.StringOutput POWER_VALUE__DOLLAR__;
        SplusTcpClient CLIENT;
        StringParameter DEVICE_IP__DOLLAR__;
        StringParameter USERNAME__DOLLAR__;
        StringParameter PASSWORD__DOLLAR__;
        short G_CONNECTIONSTATUS = 0;
        short G_SOCKETSTATUS = 0;
        CrestronString AUTH__DOLLAR__;
        private void OUTLETCONTROL (  SplusExecutionContext __context__, short NUM , short ACTION ) 
            { 
            CrestronString TARGET__DOLLAR__;
            TARGET__DOLLAR__  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 64, this );
            
            CrestronString PACKETTOSEND__DOLLAR__;
            PACKETTOSEND__DOLLAR__  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 200, this );
            
            CrestronString BUFFER__DOLLAR__;
            BUFFER__DOLLAR__  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 200, this );
            
            short RES = 0;
            
            ushort COUNT = 0;
            
            
            __context__.SourceCodeLine = 169;
            COUNT = (ushort) ( 0 ) ; 
            __context__.SourceCodeLine = 170;
            G_CONNECTIONSTATUS = (short) ( Functions.SocketConnectClient( CLIENT , DEVICE_IP__DOLLAR__  , (ushort)( 80 ) , (ushort)( 0 ) ) ) ; 
            __context__.SourceCodeLine = 173;
            while ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (G_SOCKETSTATUS != 2) ) && Functions.TestForTrue ( Functions.BoolToInt ( COUNT < 10 ) )) ))  ) ) 
                { 
                __context__.SourceCodeLine = 174;
                Functions.Delay (  (int) ( 20 ) ) ; 
                __context__.SourceCodeLine = 175;
                COUNT = (ushort) ( (COUNT + 1) ) ; 
                __context__.SourceCodeLine = 173;
                } 
            
            __context__.SourceCodeLine = 178;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (ACTION == Functions.ToSignedLongInteger( -( 1 ) )))  ) ) 
                { 
                __context__.SourceCodeLine = 179;
                Trace( "(OutletControl): Outlet Status Requested (action={0:d})", (short)ACTION) ; 
                } 
            
            else 
                {
                __context__.SourceCodeLine = 180;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (ACTION == 0))  ) ) 
                    { 
                    __context__.SourceCodeLine = 181;
                    Trace( "(OutletControl): Turn OFF Outlet #{0:d} (action={1:d})", (short)NUM, (short)ACTION) ; 
                    } 
                
                else 
                    {
                    __context__.SourceCodeLine = 182;
                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (ACTION == 1))  ) ) 
                        { 
                        __context__.SourceCodeLine = 183;
                        Trace( "(OutletControl): Turn ON Outlet #{0:d} (action={1:d})", (short)NUM, (short)ACTION) ; 
                        } 
                    
                    else 
                        {
                        __context__.SourceCodeLine = 184;
                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (ACTION == 3))  ) ) 
                            { 
                            __context__.SourceCodeLine = 185;
                            Trace( "(OutletControl): Reset Outlet #{0:d} (action={1:d})", (short)NUM, (short)ACTION) ; 
                            } 
                        
                        else 
                            {
                            __context__.SourceCodeLine = 186;
                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (ACTION == 4))  ) ) 
                                { 
                                __context__.SourceCodeLine = 187;
                                Trace( "(OutletControl): Auto Reboot ON") ; 
                                } 
                            
                            else 
                                {
                                __context__.SourceCodeLine = 188;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (ACTION == 5))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 189;
                                    Trace( "(OutletControl): Auto Reboot OFF") ; 
                                    } 
                                
                                }
                            
                            }
                        
                        }
                    
                    }
                
                }
            
            __context__.SourceCodeLine = 192;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (ACTION != Functions.ToSignedLongInteger( -( 1 ) )))  ) ) 
                { 
                __context__.SourceCodeLine = 193;
                MakeString ( TARGET__DOLLAR__ , "/control.cgi?outlet={0:d}&command={1:d}", (short)NUM, (short)ACTION) ; 
                __context__.SourceCodeLine = 194;
                MakeString ( PACKETTOSEND__DOLLAR__ , "GET {0} HTTP/1.1\r\nHost: {1}\r\nKeep-Alive: 300\r\nConnection: keep-alive\r\nAuthorization: Basic {2}\r\nUser-Agent: APP\r\n\r\n", TARGET__DOLLAR__ , DEVICE_IP__DOLLAR__ , AUTH__DOLLAR__ ) ; 
                __context__.SourceCodeLine = 195;
                RES = (short) ( Functions.SocketSend( CLIENT , PACKETTOSEND__DOLLAR__ ) ) ; 
                } 
            
            else 
                { 
                __context__.SourceCodeLine = 197;
                MakeString ( TARGET__DOLLAR__ , "/wattbox_info.xml") ; 
                __context__.SourceCodeLine = 198;
                MakeString ( PACKETTOSEND__DOLLAR__ , "GET {0} HTTP/1.1\r\nHost: {1}\r\nKeep-Alive: 300\r\nConnection: keep-alive\r\nAuthorization: Basic {2}\r\nUser-Agent: APP\r\n\r\n", TARGET__DOLLAR__ , DEVICE_IP__DOLLAR__ , AUTH__DOLLAR__ ) ; 
                __context__.SourceCodeLine = 199;
                RES = (short) ( Functions.SocketSend( CLIENT , PACKETTOSEND__DOLLAR__ ) ) ; 
                } 
            
            
            }
            
        object RESET_ALL_OUTLETS_OnPush_0 ( Object __EventInfo__ )
        
            { 
            Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
            try
            {
                SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
                
                __context__.SourceCodeLine = 233;
                OUTLETCONTROL (  __context__ , (short)( 0 ), (short)( 3 )) ; 
                
                
            }
            catch(Exception e) { ObjectCatchHandler(e); }
            finally { ObjectFinallyHandler( __SignalEventArg__ ); }
            return this;
            
        }
        
    object GET_STATUS_OnPush_1 ( Object __EventInfo__ )
    
        { 
        Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
        try
        {
            SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
            
            __context__.SourceCodeLine = 239;
            OUTLETCONTROL (  __context__ , (short)( Functions.ToSignedInteger( -( 1 ) ) ), (short)( Functions.ToSignedInteger( -( 1 ) ) )) ; 
            
            
        }
        catch(Exception e) { ObjectCatchHandler(e); }
        finally { ObjectFinallyHandler( __SignalEventArg__ ); }
        return this;
        
    }
    
object AUTO_REBOOT_ON_OnPush_2 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 244;
        OUTLETCONTROL (  __context__ , (short)( 0 ), (short)( 4 )) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object AUTO_REBOOT_OFF_OnPush_3 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 249;
        OUTLETCONTROL (  __context__ , (short)( 0 ), (short)( 5 )) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object OUTLET_ON_OnPush_4 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        ushort N = 0;
        
        
        __context__.SourceCodeLine = 255;
        N = (ushort) ( Functions.GetLastModifiedArrayIndex( __SignalEventArg__ ) ) ; 
        __context__.SourceCodeLine = 256;
        OUTLETCONTROL (  __context__ , (short)( N ), (short)( 1 )) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object OUTLET_OFF_OnPush_5 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        ushort N = 0;
        
        
        __context__.SourceCodeLine = 262;
        N = (ushort) ( Functions.GetLastModifiedArrayIndex( __SignalEventArg__ ) ) ; 
        __context__.SourceCodeLine = 263;
        OUTLETCONTROL (  __context__ , (short)( N ), (short)( 0 )) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object OUTLET_RESET_OnPush_6 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        ushort N = 0;
        
        
        __context__.SourceCodeLine = 269;
        N = (ushort) ( Functions.GetLastModifiedArrayIndex( __SignalEventArg__ ) ) ; 
        __context__.SourceCodeLine = 270;
        OUTLETCONTROL (  __context__ , (short)( N ), (short)( 3 )) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object CLIENT_OnSocketConnect_7 ( Object __Info__ )

    { 
    SocketEventInfo __SocketInfo__ = (SocketEventInfo)__Info__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SocketInfo__);
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SocketInfo__ ); }
    return this;
    
}

object CLIENT_OnSocketDisconnect_8 ( Object __Info__ )

    { 
    SocketEventInfo __SocketInfo__ = (SocketEventInfo)__Info__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SocketInfo__);
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SocketInfo__ ); }
    return this;
    
}

object CLIENT_OnSocketReceive_9 ( Object __Info__ )

    { 
    SocketEventInfo __SocketInfo__ = (SocketEventInfo)__Info__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SocketInfo__);
        CrestronString IN__DOLLAR__;
        IN__DOLLAR__  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 16384, this );
        
        CrestronString RX_HOST_NAME__DOLLAR__;
        RX_HOST_NAME__DOLLAR__  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 32, this );
        
        CrestronString RX_HARDWARE_VERSION__DOLLAR__;
        RX_HARDWARE_VERSION__DOLLAR__  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 32, this );
        
        CrestronString RX_SERIAL_NUMBER__DOLLAR__;
        RX_SERIAL_NUMBER__DOLLAR__  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 32, this );
        
        CrestronString RX_SAFE_VOLTAGE_STATUS__DOLLAR__;
        RX_SAFE_VOLTAGE_STATUS__DOLLAR__  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 32, this );
        
        CrestronString RX_VOLTAGE_VALUE__DOLLAR__;
        RX_VOLTAGE_VALUE__DOLLAR__  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 32, this );
        
        CrestronString RX_CURRENT_VALUE__DOLLAR__;
        RX_CURRENT_VALUE__DOLLAR__  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 32, this );
        
        CrestronString RX_POWER_VALUE__DOLLAR__;
        RX_POWER_VALUE__DOLLAR__  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 32, this );
        
        CrestronString RX_OUTLET_STATUS__DOLLAR__;
        RX_OUTLET_STATUS__DOLLAR__  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 32, this );
        
        CrestronString OPEN_TAG__DOLLAR__;
        CrestronString CLOSE_TAG__DOLLAR__;
        OPEN_TAG__DOLLAR__  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 32, this );
        CLOSE_TAG__DOLLAR__  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 32, this );
        
        CrestronString [] TAGS__DOLLAR__;
        TAGS__DOLLAR__  = new CrestronString[ 11 ];
        for( uint i = 0; i < 11; i++ )
            TAGS__DOLLAR__ [i] = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 32, this );
        
        ushort TAG_START = 0;
        ushort TAG_END = 0;
        ushort START = 0;
        ushort END = 0;
        
        ushort N_TAGS = 0;
        
        ushort I = 0;
        ushort Z = 0;
        ushort OUTLET_COUNT = 0;
        
        ushort IS_STATUS = 0;
        
        ushort INCHAR = 0;
        
        ushort [] OUTLET_STATUS_ARRAY;
        OUTLET_STATUS_ARRAY  = new ushort[ 33 ];
        
        
        __context__.SourceCodeLine = 323;
        TAGS__DOLLAR__ [ 0 ]  .UpdateValue ( "host_name"  ) ; 
        __context__.SourceCodeLine = 324;
        TAGS__DOLLAR__ [ 1 ]  .UpdateValue ( "hardware_version"  ) ; 
        __context__.SourceCodeLine = 325;
        TAGS__DOLLAR__ [ 2 ]  .UpdateValue ( "serial_number"  ) ; 
        __context__.SourceCodeLine = 326;
        TAGS__DOLLAR__ [ 3 ]  .UpdateValue ( "safe_voltage_status"  ) ; 
        __context__.SourceCodeLine = 327;
        TAGS__DOLLAR__ [ 4 ]  .UpdateValue ( "voltage_value"  ) ; 
        __context__.SourceCodeLine = 328;
        TAGS__DOLLAR__ [ 5 ]  .UpdateValue ( "current_value"  ) ; 
        __context__.SourceCodeLine = 329;
        TAGS__DOLLAR__ [ 6 ]  .UpdateValue ( "power_value"  ) ; 
        __context__.SourceCodeLine = 330;
        TAGS__DOLLAR__ [ 7 ]  .UpdateValue ( "outlet_status"  ) ; 
        __context__.SourceCodeLine = 332;
        N_TAGS = (ushort) ( Functions.GetNumArrayRows( TAGS__DOLLAR__ ) ) ; 
        __context__.SourceCodeLine = 334;
        IN__DOLLAR__  .UpdateValue ( CLIENT .  SocketRxBuf  ) ; 
        __context__.SourceCodeLine = 340;
        IS_STATUS = (ushort) ( Functions.Find( "host_name" , IN__DOLLAR__ , 1 ) ) ; 
        __context__.SourceCodeLine = 342;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (IS_STATUS != 0))  ) ) 
            { 
            __context__.SourceCodeLine = 343;
            Trace( "(RxSocket): Outlet Status Request") ; 
            __context__.SourceCodeLine = 344;
            ushort __FN_FORSTART_VAL__1 = (ushort) ( 0 ) ;
            ushort __FN_FOREND_VAL__1 = (ushort)N_TAGS; 
            int __FN_FORSTEP_VAL__1 = (int)1; 
            for ( I  = __FN_FORSTART_VAL__1; (__FN_FORSTEP_VAL__1 > 0)  ? ( (I  >= __FN_FORSTART_VAL__1) && (I  <= __FN_FOREND_VAL__1) ) : ( (I  <= __FN_FORSTART_VAL__1) && (I  >= __FN_FOREND_VAL__1) ) ; I  += (ushort)__FN_FORSTEP_VAL__1) 
                { 
                __context__.SourceCodeLine = 345;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (TAGS__DOLLAR__[ I ] != ""))  ) ) 
                    { 
                    __context__.SourceCodeLine = 346;
                    MakeString ( OPEN_TAG__DOLLAR__ , "<{0}>", TAGS__DOLLAR__ [ I ] ) ; 
                    __context__.SourceCodeLine = 347;
                    MakeString ( CLOSE_TAG__DOLLAR__ , "</{0}>", TAGS__DOLLAR__ [ I ] ) ; 
                    __context__.SourceCodeLine = 350;
                    TAG_START = (ushort) ( Functions.Find( OPEN_TAG__DOLLAR__ , IN__DOLLAR__ , 1 ) ) ; 
                    __context__.SourceCodeLine = 351;
                    TAG_END = (ushort) ( Functions.Find( CLOSE_TAG__DOLLAR__ , IN__DOLLAR__ , 1 ) ) ; 
                    __context__.SourceCodeLine = 353;
                    START = (ushort) ( (TAG_START + Functions.Length( OPEN_TAG__DOLLAR__ )) ) ; 
                    __context__.SourceCodeLine = 354;
                    END = (ushort) ( ((TAG_END - TAG_START) - Functions.Length( OPEN_TAG__DOLLAR__ )) ) ; 
                    __context__.SourceCodeLine = 356;
                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (TAGS__DOLLAR__[ I ] == "host_name"))  ) ) 
                        { 
                        __context__.SourceCodeLine = 357;
                        RX_HOST_NAME__DOLLAR__  .UpdateValue ( Functions.Mid ( IN__DOLLAR__ ,  (int) ( START ) ,  (int) ( END ) )  ) ; 
                        __context__.SourceCodeLine = 358;
                        HOST_NAME__DOLLAR__  .UpdateValue ( RX_HOST_NAME__DOLLAR__  ) ; 
                        __context__.SourceCodeLine = 359;
                        Trace( "(RxSocket) host_name: {0}", RX_HOST_NAME__DOLLAR__ ) ; 
                        } 
                    
                    else 
                        {
                        __context__.SourceCodeLine = 360;
                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (TAGS__DOLLAR__[ I ] == "hardware_version"))  ) ) 
                            { 
                            __context__.SourceCodeLine = 361;
                            RX_HARDWARE_VERSION__DOLLAR__  .UpdateValue ( Functions.Mid ( IN__DOLLAR__ ,  (int) ( START ) ,  (int) ( END ) )  ) ; 
                            __context__.SourceCodeLine = 362;
                            HARDWARE_VERSION__DOLLAR__  .UpdateValue ( RX_HARDWARE_VERSION__DOLLAR__  ) ; 
                            __context__.SourceCodeLine = 363;
                            Trace( "(RxSocket) hardware_version: {0}", RX_HARDWARE_VERSION__DOLLAR__ ) ; 
                            } 
                        
                        else 
                            {
                            __context__.SourceCodeLine = 364;
                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (TAGS__DOLLAR__[ I ] == "serial_number"))  ) ) 
                                { 
                                __context__.SourceCodeLine = 365;
                                RX_SERIAL_NUMBER__DOLLAR__  .UpdateValue ( Functions.Mid ( IN__DOLLAR__ ,  (int) ( START ) ,  (int) ( END ) )  ) ; 
                                __context__.SourceCodeLine = 366;
                                SERIAL_NUMBER__DOLLAR__  .UpdateValue ( RX_SERIAL_NUMBER__DOLLAR__  ) ; 
                                __context__.SourceCodeLine = 367;
                                Trace( "(RxSocket) serial_number: {0}", RX_SERIAL_NUMBER__DOLLAR__ ) ; 
                                } 
                            
                            else 
                                {
                                __context__.SourceCodeLine = 368;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (TAGS__DOLLAR__[ I ] == "safe_voltage_status"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 369;
                                    RX_SAFE_VOLTAGE_STATUS__DOLLAR__  .UpdateValue ( Functions.Mid ( IN__DOLLAR__ ,  (int) ( START ) ,  (int) ( END ) )  ) ; 
                                    __context__.SourceCodeLine = 370;
                                    SAFE_VOLTAGE_STATUS__DOLLAR__  .UpdateValue ( RX_SAFE_VOLTAGE_STATUS__DOLLAR__  ) ; 
                                    __context__.SourceCodeLine = 371;
                                    Trace( "(RxSocket) safe_voltage_status: {0}", RX_SAFE_VOLTAGE_STATUS__DOLLAR__ ) ; 
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 372;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (TAGS__DOLLAR__[ I ] == "voltage_value"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 373;
                                        RX_VOLTAGE_VALUE__DOLLAR__  .UpdateValue ( Functions.Mid ( IN__DOLLAR__ ,  (int) ( START ) ,  (int) ( END ) )  ) ; 
                                        __context__.SourceCodeLine = 374;
                                        VOLTAGE_VALUE__DOLLAR__  .UpdateValue ( RX_VOLTAGE_VALUE__DOLLAR__  ) ; 
                                        __context__.SourceCodeLine = 375;
                                        Trace( "(RxSocket) voltage_value: {0}", RX_VOLTAGE_VALUE__DOLLAR__ ) ; 
                                        } 
                                    
                                    else 
                                        {
                                        __context__.SourceCodeLine = 376;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (TAGS__DOLLAR__[ I ] == "current_value"))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 377;
                                            RX_CURRENT_VALUE__DOLLAR__  .UpdateValue ( Functions.Mid ( IN__DOLLAR__ ,  (int) ( START ) ,  (int) ( END ) )  ) ; 
                                            __context__.SourceCodeLine = 378;
                                            CURRENT_VALUE__DOLLAR__  .UpdateValue ( RX_CURRENT_VALUE__DOLLAR__  ) ; 
                                            __context__.SourceCodeLine = 379;
                                            Trace( "(RxSocket) current_value: {0}", RX_CURRENT_VALUE__DOLLAR__ ) ; 
                                            } 
                                        
                                        else 
                                            {
                                            __context__.SourceCodeLine = 380;
                                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (TAGS__DOLLAR__[ I ] == "power_value"))  ) ) 
                                                { 
                                                __context__.SourceCodeLine = 381;
                                                RX_POWER_VALUE__DOLLAR__  .UpdateValue ( Functions.Mid ( IN__DOLLAR__ ,  (int) ( START ) ,  (int) ( END ) )  ) ; 
                                                __context__.SourceCodeLine = 382;
                                                POWER_VALUE__DOLLAR__  .UpdateValue ( RX_POWER_VALUE__DOLLAR__  ) ; 
                                                __context__.SourceCodeLine = 383;
                                                Trace( "(RxSocket) power_value: {0}", RX_POWER_VALUE__DOLLAR__ ) ; 
                                                } 
                                            
                                            else 
                                                {
                                                __context__.SourceCodeLine = 384;
                                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (TAGS__DOLLAR__[ I ] == "outlet_status"))  ) ) 
                                                    { 
                                                    __context__.SourceCodeLine = 386;
                                                    RX_OUTLET_STATUS__DOLLAR__  .UpdateValue ( Functions.Mid ( IN__DOLLAR__ ,  (int) ( START ) ,  (int) ( END ) ) + "$"  ) ; 
                                                    __context__.SourceCodeLine = 387;
                                                    Trace( "(RxSocket): outlet_status: {0}", RX_OUTLET_STATUS__DOLLAR__ ) ; 
                                                    __context__.SourceCodeLine = 388;
                                                    INCHAR = (ushort) ( 0 ) ; 
                                                    __context__.SourceCodeLine = 389;
                                                    Z = (ushort) ( 0 ) ; 
                                                    __context__.SourceCodeLine = 391;
                                                    while ( Functions.TestForTrue  ( ( Functions.BoolToInt (INCHAR != 36))  ) ) 
                                                        { 
                                                        __context__.SourceCodeLine = 392;
                                                        INCHAR = (ushort) ( Functions.GetC( RX_OUTLET_STATUS__DOLLAR__ ) ) ; 
                                                        __context__.SourceCodeLine = 393;
                                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (INCHAR != 44) ) && Functions.TestForTrue ( Functions.BoolToInt (INCHAR != 36) )) ))  ) ) 
                                                            { 
                                                            __context__.SourceCodeLine = 394;
                                                            OUTLET_STATUS_ARRAY [ Z] = (ushort) ( INCHAR ) ; 
                                                            __context__.SourceCodeLine = 395;
                                                            Z = (ushort) ( (Z + 1) ) ; 
                                                            } 
                                                        
                                                        __context__.SourceCodeLine = 391;
                                                        } 
                                                    
                                                    __context__.SourceCodeLine = 399;
                                                    OUTLET_COUNT = (ushort) ( Z ) ; 
                                                    __context__.SourceCodeLine = 402;
                                                    ushort __FN_FORSTART_VAL__2 = (ushort) ( 0 ) ;
                                                    ushort __FN_FOREND_VAL__2 = (ushort)(OUTLET_COUNT - 1); 
                                                    int __FN_FORSTEP_VAL__2 = (int)1; 
                                                    for ( Z  = __FN_FORSTART_VAL__2; (__FN_FORSTEP_VAL__2 > 0)  ? ( (Z  >= __FN_FORSTART_VAL__2) && (Z  <= __FN_FOREND_VAL__2) ) : ( (Z  <= __FN_FORSTART_VAL__2) && (Z  >= __FN_FOREND_VAL__2) ) ; Z  += (ushort)__FN_FORSTEP_VAL__2) 
                                                        { 
                                                        __context__.SourceCodeLine = 403;
                                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (OUTLET_STATUS_ARRAY[ Z ] == 49))  ) ) 
                                                            { 
                                                            __context__.SourceCodeLine = 404;
                                                            OUTLET_STATUS_ARRAY [ Z] = (ushort) ( 1 ) ; 
                                                            } 
                                                        
                                                        else 
                                                            {
                                                            __context__.SourceCodeLine = 405;
                                                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (OUTLET_STATUS_ARRAY[ Z ] == 48))  ) ) 
                                                                { 
                                                                __context__.SourceCodeLine = 406;
                                                                OUTLET_STATUS_ARRAY [ Z] = (ushort) ( 0 ) ; 
                                                                } 
                                                            
                                                            }
                                                        
                                                        __context__.SourceCodeLine = 402;
                                                        } 
                                                    
                                                    __context__.SourceCodeLine = 410;
                                                    ushort __FN_FORSTART_VAL__3 = (ushort) ( 1 ) ;
                                                    ushort __FN_FOREND_VAL__3 = (ushort)OUTLET_COUNT; 
                                                    int __FN_FORSTEP_VAL__3 = (int)1; 
                                                    for ( Z  = __FN_FORSTART_VAL__3; (__FN_FORSTEP_VAL__3 > 0)  ? ( (Z  >= __FN_FORSTART_VAL__3) && (Z  <= __FN_FOREND_VAL__3) ) : ( (Z  <= __FN_FORSTART_VAL__3) && (Z  >= __FN_FOREND_VAL__3) ) ; Z  += (ushort)__FN_FORSTEP_VAL__3) 
                                                        { 
                                                        __context__.SourceCodeLine = 411;
                                                        OUTLET_STATUS [ Z]  .Value = (ushort) ( OUTLET_STATUS_ARRAY[ (Z - 1) ] ) ; 
                                                        __context__.SourceCodeLine = 410;
                                                        } 
                                                    
                                                    } 
                                                
                                                }
                                            
                                            }
                                        
                                        }
                                    
                                    }
                                
                                }
                            
                            }
                        
                        }
                    
                    } 
                
                __context__.SourceCodeLine = 344;
                } 
            
            } 
        
        else 
            { 
            __context__.SourceCodeLine = 417;
            Functions.Delay (  (int) ( 10 ) ) ; 
            __context__.SourceCodeLine = 418;
            Functions.ClearBuffer ( CLIENT .  SocketRxBuf ) ; 
            __context__.SourceCodeLine = 419;
            G_CONNECTIONSTATUS = (short) ( Functions.SocketDisconnectClient( CLIENT ) ) ; 
            __context__.SourceCodeLine = 420;
            OUTLETCONTROL (  __context__ , (short)( Functions.ToSignedInteger( -( 1 ) ) ), (short)( Functions.ToSignedInteger( -( 1 ) ) )) ; 
            __context__.SourceCodeLine = 421;
            return  this ; 
            } 
        
        __context__.SourceCodeLine = 423;
        Functions.ClearBuffer ( CLIENT .  SocketRxBuf ) ; 
        __context__.SourceCodeLine = 424;
        G_CONNECTIONSTATUS = (short) ( Functions.SocketDisconnectClient( CLIENT ) ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SocketInfo__ ); }
    return this;
    
}

object CLIENT_OnSocketStatus_10 ( Object __Info__ )

    { 
    SocketEventInfo __SocketInfo__ = (SocketEventInfo)__Info__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SocketInfo__);
        
        __context__.SourceCodeLine = 430;
        G_SOCKETSTATUS = (short) ( __SocketInfo__.SocketStatus ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SocketInfo__ ); }
    return this;
    
}

public override object FunctionMain (  object __obj__ ) 
    { 
    try
    {
        SplusExecutionContext __context__ = SplusFunctionMainStartCode();
        
        __context__.SourceCodeLine = 444;
        Trace( "SnapAV Wattbox v1.0 Surge Protector - Initializing...") ; 
        __context__.SourceCodeLine = 446;
        MakeString ( AUTH__DOLLAR__ , "{0}:{1}", USERNAME__DOLLAR__ , PASSWORD__DOLLAR__ ) ; 
        __context__.SourceCodeLine = 447;
        AUTH__DOLLAR__  .UpdateValue ( UserModuleClass_BASE64.GETBASE64 (  __context__ , this , AUTH__DOLLAR__)  ) ; 
        __context__.SourceCodeLine = 448;
        OUTLETCONTROL (  __context__ , (short)( Functions.ToSignedInteger( -( 1 ) ) ), (short)( Functions.ToSignedInteger( -( 1 ) ) )) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler(); }
    return __obj__;
    }
    

public override void LogosSplusInitialize()
{
    _SplusNVRAM = new SplusNVRAM( this );
    AUTH__DOLLAR__  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 64, this );
    CLIENT  = new SplusTcpClient ( 16384, this );
    
    RESET_ALL_OUTLETS = new Crestron.Logos.SplusObjects.DigitalInput( RESET_ALL_OUTLETS__DigitalInput__, this );
    m_DigitalInputList.Add( RESET_ALL_OUTLETS__DigitalInput__, RESET_ALL_OUTLETS );
    
    AUTO_REBOOT_ON = new Crestron.Logos.SplusObjects.DigitalInput( AUTO_REBOOT_ON__DigitalInput__, this );
    m_DigitalInputList.Add( AUTO_REBOOT_ON__DigitalInput__, AUTO_REBOOT_ON );
    
    AUTO_REBOOT_OFF = new Crestron.Logos.SplusObjects.DigitalInput( AUTO_REBOOT_OFF__DigitalInput__, this );
    m_DigitalInputList.Add( AUTO_REBOOT_OFF__DigitalInput__, AUTO_REBOOT_OFF );
    
    GET_STATUS = new Crestron.Logos.SplusObjects.DigitalInput( GET_STATUS__DigitalInput__, this );
    m_DigitalInputList.Add( GET_STATUS__DigitalInput__, GET_STATUS );
    
    OUTLET_ON = new InOutArray<DigitalInput>( 32, this );
    for( uint i = 0; i < 32; i++ )
    {
        OUTLET_ON[i+1] = new Crestron.Logos.SplusObjects.DigitalInput( OUTLET_ON__DigitalInput__ + i, OUTLET_ON__DigitalInput__, this );
        m_DigitalInputList.Add( OUTLET_ON__DigitalInput__ + i, OUTLET_ON[i+1] );
    }
    
    OUTLET_OFF = new InOutArray<DigitalInput>( 32, this );
    for( uint i = 0; i < 32; i++ )
    {
        OUTLET_OFF[i+1] = new Crestron.Logos.SplusObjects.DigitalInput( OUTLET_OFF__DigitalInput__ + i, OUTLET_OFF__DigitalInput__, this );
        m_DigitalInputList.Add( OUTLET_OFF__DigitalInput__ + i, OUTLET_OFF[i+1] );
    }
    
    OUTLET_RESET = new InOutArray<DigitalInput>( 32, this );
    for( uint i = 0; i < 32; i++ )
    {
        OUTLET_RESET[i+1] = new Crestron.Logos.SplusObjects.DigitalInput( OUTLET_RESET__DigitalInput__ + i, OUTLET_RESET__DigitalInput__, this );
        m_DigitalInputList.Add( OUTLET_RESET__DigitalInput__ + i, OUTLET_RESET[i+1] );
    }
    
    OUTLET_STATUS = new InOutArray<DigitalOutput>( 32, this );
    for( uint i = 0; i < 32; i++ )
    {
        OUTLET_STATUS[i+1] = new Crestron.Logos.SplusObjects.DigitalOutput( OUTLET_STATUS__DigitalOutput__ + i, this );
        m_DigitalOutputList.Add( OUTLET_STATUS__DigitalOutput__ + i, OUTLET_STATUS[i+1] );
    }
    
    HOST_NAME__DOLLAR__ = new Crestron.Logos.SplusObjects.StringOutput( HOST_NAME__DOLLAR____AnalogSerialOutput__, this );
    m_StringOutputList.Add( HOST_NAME__DOLLAR____AnalogSerialOutput__, HOST_NAME__DOLLAR__ );
    
    HARDWARE_VERSION__DOLLAR__ = new Crestron.Logos.SplusObjects.StringOutput( HARDWARE_VERSION__DOLLAR____AnalogSerialOutput__, this );
    m_StringOutputList.Add( HARDWARE_VERSION__DOLLAR____AnalogSerialOutput__, HARDWARE_VERSION__DOLLAR__ );
    
    SERIAL_NUMBER__DOLLAR__ = new Crestron.Logos.SplusObjects.StringOutput( SERIAL_NUMBER__DOLLAR____AnalogSerialOutput__, this );
    m_StringOutputList.Add( SERIAL_NUMBER__DOLLAR____AnalogSerialOutput__, SERIAL_NUMBER__DOLLAR__ );
    
    SAFE_VOLTAGE_STATUS__DOLLAR__ = new Crestron.Logos.SplusObjects.StringOutput( SAFE_VOLTAGE_STATUS__DOLLAR____AnalogSerialOutput__, this );
    m_StringOutputList.Add( SAFE_VOLTAGE_STATUS__DOLLAR____AnalogSerialOutput__, SAFE_VOLTAGE_STATUS__DOLLAR__ );
    
    VOLTAGE_VALUE__DOLLAR__ = new Crestron.Logos.SplusObjects.StringOutput( VOLTAGE_VALUE__DOLLAR____AnalogSerialOutput__, this );
    m_StringOutputList.Add( VOLTAGE_VALUE__DOLLAR____AnalogSerialOutput__, VOLTAGE_VALUE__DOLLAR__ );
    
    CURRENT_VALUE__DOLLAR__ = new Crestron.Logos.SplusObjects.StringOutput( CURRENT_VALUE__DOLLAR____AnalogSerialOutput__, this );
    m_StringOutputList.Add( CURRENT_VALUE__DOLLAR____AnalogSerialOutput__, CURRENT_VALUE__DOLLAR__ );
    
    POWER_VALUE__DOLLAR__ = new Crestron.Logos.SplusObjects.StringOutput( POWER_VALUE__DOLLAR____AnalogSerialOutput__, this );
    m_StringOutputList.Add( POWER_VALUE__DOLLAR____AnalogSerialOutput__, POWER_VALUE__DOLLAR__ );
    
    DEVICE_IP__DOLLAR__ = new StringParameter( DEVICE_IP__DOLLAR____Parameter__, this );
    m_ParameterList.Add( DEVICE_IP__DOLLAR____Parameter__, DEVICE_IP__DOLLAR__ );
    
    USERNAME__DOLLAR__ = new StringParameter( USERNAME__DOLLAR____Parameter__, this );
    m_ParameterList.Add( USERNAME__DOLLAR____Parameter__, USERNAME__DOLLAR__ );
    
    PASSWORD__DOLLAR__ = new StringParameter( PASSWORD__DOLLAR____Parameter__, this );
    m_ParameterList.Add( PASSWORD__DOLLAR____Parameter__, PASSWORD__DOLLAR__ );
    
    
    RESET_ALL_OUTLETS.OnDigitalPush.Add( new InputChangeHandlerWrapper( RESET_ALL_OUTLETS_OnPush_0, false ) );
    GET_STATUS.OnDigitalPush.Add( new InputChangeHandlerWrapper( GET_STATUS_OnPush_1, false ) );
    AUTO_REBOOT_ON.OnDigitalPush.Add( new InputChangeHandlerWrapper( AUTO_REBOOT_ON_OnPush_2, false ) );
    AUTO_REBOOT_OFF.OnDigitalPush.Add( new InputChangeHandlerWrapper( AUTO_REBOOT_OFF_OnPush_3, false ) );
    for( uint i = 0; i < 32; i++ )
        OUTLET_ON[i+1].OnDigitalPush.Add( new InputChangeHandlerWrapper( OUTLET_ON_OnPush_4, false ) );
        
    for( uint i = 0; i < 32; i++ )
        OUTLET_OFF[i+1].OnDigitalPush.Add( new InputChangeHandlerWrapper( OUTLET_OFF_OnPush_5, false ) );
        
    for( uint i = 0; i < 32; i++ )
        OUTLET_RESET[i+1].OnDigitalPush.Add( new InputChangeHandlerWrapper( OUTLET_RESET_OnPush_6, false ) );
        
    CLIENT.OnSocketConnect.Add( new SocketHandlerWrapper( CLIENT_OnSocketConnect_7, false ) );
    CLIENT.OnSocketDisconnect.Add( new SocketHandlerWrapper( CLIENT_OnSocketDisconnect_8, false ) );
    CLIENT.OnSocketReceive.Add( new SocketHandlerWrapper( CLIENT_OnSocketReceive_9, false ) );
    CLIENT.OnSocketStatus.Add( new SocketHandlerWrapper( CLIENT_OnSocketStatus_10, false ) );
    
    _SplusNVRAM.PopulateCustomAttributeList( true );
    
    NVRAM = _SplusNVRAM;
    
}

public override void LogosSimplSharpInitialize()
{
    
    
}

public UserModuleClass_SNAPAV_WATTBOX_V1_0 ( string InstanceName, string ReferenceID, Crestron.Logos.SplusObjects.CrestronStringEncoding nEncodingType ) : base( InstanceName, ReferenceID, nEncodingType ) {}




const uint RESET_ALL_OUTLETS__DigitalInput__ = 0;
const uint AUTO_REBOOT_ON__DigitalInput__ = 1;
const uint AUTO_REBOOT_OFF__DigitalInput__ = 2;
const uint GET_STATUS__DigitalInput__ = 3;
const uint OUTLET_ON__DigitalInput__ = 4;
const uint OUTLET_OFF__DigitalInput__ = 36;
const uint OUTLET_RESET__DigitalInput__ = 68;
const uint OUTLET_STATUS__DigitalOutput__ = 0;
const uint HOST_NAME__DOLLAR____AnalogSerialOutput__ = 0;
const uint HARDWARE_VERSION__DOLLAR____AnalogSerialOutput__ = 1;
const uint SERIAL_NUMBER__DOLLAR____AnalogSerialOutput__ = 2;
const uint SAFE_VOLTAGE_STATUS__DOLLAR____AnalogSerialOutput__ = 3;
const uint VOLTAGE_VALUE__DOLLAR____AnalogSerialOutput__ = 4;
const uint CURRENT_VALUE__DOLLAR____AnalogSerialOutput__ = 5;
const uint POWER_VALUE__DOLLAR____AnalogSerialOutput__ = 6;
const uint DEVICE_IP__DOLLAR____Parameter__ = 10;
const uint USERNAME__DOLLAR____Parameter__ = 11;
const uint PASSWORD__DOLLAR____Parameter__ = 12;

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
