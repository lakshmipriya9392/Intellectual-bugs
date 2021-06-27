import React from 'react'
import { Link } from 'react-router-dom'
import notification from './../../assets/notification.png'
import { motion } from 'framer-motion'


function navbar(props) {
    let notificationCount = 2
    return (
        <>
            <div className="sticky top-0
     left-0 right-0 text-center py-3 bg-blue-500 
      text-white flex justify-between md:px-20 px-8 z-40">

                <div className="flex my-auto">

                    <Link to='/'>
                        <span className="my-auto md:text-3xl text-2xl mx-5">Training lab</span>
                    </Link>

                </div>

                <span className="flex">
                    <div className="bg-green-500 inline-block mx-5 p-5 rounded-full cursor-pointer"></div>

                    <Link to="/events">
                        <div className="flex">
                            <img src={notification} alt="notification"
                                className="cursor-pointer w-8 h-8 my-auto" />
                            <span className="text-base">{notificationCount}</span></div>
                    </Link>
                </span>
            </div>
        </>
    )
}

export default navbar
