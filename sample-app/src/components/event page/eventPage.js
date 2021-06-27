// import { render } from '@testing-library/react'
import React, { useState, useEffect } from 'react'
import './../../App.css'
import Navbar from '../navbar/navbar'
import axios from 'axios'
import { motion } from 'framer-motion'


function Eventpage() {

    //getting data from API

    const url = "https://localhost:5001/";

    const getEvents = () => {
        axios.get(`${url}event`)
            .then((response) => {
                const allEvents = response.data;
                setEvents(allEvents);
            }).catch(error => console.log(`Error : ${error}`))
    }

    useEffect(() => {
        getEvents()
    }, []);

    const [events, setEvents] = useState([]);




    // let [transpile, transcription] = useState(true)
    const scrollnum = 1000

    const toLeft = () => {
        const scroller = document.querySelector('#scrollbox')
        scroller.scrollLeft -= scrollnum
    }

    const toRight = () => {
        const scroller = document.querySelector('#scrollbox')
        scroller.scrollLeft += scrollnum
    }


    return (
        <div className='overflow-hidden'>

            <motion.div
                initial={{ scale: 0 }}
                animate={{ scale: 1 }}
                transition={{ delay: 1 }}
                className='relative top-0 right-0 left-0 bottom-0 bg-blue-300 '
            >

                {/* The below code is of top navbar */}

                <Navbar />

                {/* The below code is of the main display page */}


                <div className="text-center text-4xl my-8 text-white">List of all events below</div>

                <div className="my-2 flex justify-center items-center flex-col">


                    {events.map(eve => {
                        return <div key={eve.id} className="w-10/12 h-auto bg-white flex flex-col rounded-lg m-4 shadow-2xl">
                            <iframe width="100%" height="380rem" src='http://localhost:5500/videos/events/1.mp4'
                                title="YouTube video player" frameBorder="0" allow="accelerometer;
                             clipboard-write; encrypted-media; gyroscope; picture-in-picture" allowFullScreen
                                className='rounded-t-lg'></iframe>
                            <div className="text-3xl text-center my-2">{eve.eventName}</div>
                            <div className="flex justify-between mx-5">
                                <div className="mx-5 text-2xl">Start : {eve.startTime}</div>
                                <div className="mx-5 text-2xl">End : {eve.endTime}</div>
                            </div>
                            <div className="text-xl mx-5 mt-2 mb-5">
                                {eve.description}
                            </div>
                        </div>
                    })}

                </div>


                <footer className="bg-gray-800 px-5 py-10
                  text-white text-center text-xl mt-5 relative bottom-0">
                    All rights reserved
                </footer>
            </motion.div>

        </div >
    )
}

export default Eventpage