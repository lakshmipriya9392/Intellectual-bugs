import React from 'react'
import BackImage from './../../assets/Background_design_(8).jpg'
import TechImage from './../../assets/TechImage.jpeg'
import TestImage from './../../assets/TestImage.png'
import EventImage from './../../assets/EventImage.jpg'
import { motion } from 'framer-motion'
import Navbar from './../navbar/navbar'
import { Link } from 'react-router-dom'

function SelectionPage() {
    return (
        <div className="absolute overflow-x-hidden w-screen h-full">
            <motion.div
                initial={{ marginLeft: '100vw' }}
                animate={{ marginLeft: 0 }}
                transition={{ duration: 0.5, type: 'spring', stiffness: 150, delay: 0.2 }}
                className='relative '>

                <img src={BackImage} alt="" className='absolute top-0 bottom-0 left-0 right-0 w-screen h-screen mx-auto mt-0 z-0' />

                <Navbar />

                <div className=" mt-48 w-full text-2xl absolute top-0 bottom-0 flex
             flex-wrap justify-center z-20">

                    <Link to='/courses'>
                        <motion.div
                            whileHover={{ scale: 1.05 }}
                            whileTap={{ scale: 0.95 }}
                            className="m-5 bg-white min-w-min w-96 h-72 rounded-3xl cursor-pointer flex flex-col shadow-2xl">
                            <img src={TechImage} alt="" className='rounded-t-3xl h-3/5' />
                            <div className=" text-center my-5">Courses</div>
                        </motion.div>
                    </Link>

                    <Link to='/test'>
                        <motion.div
                            whileHover={{ scale: 1.05 }}
                            whileTap={{ scale: 0.95 }}
                            className="m-5 bg-white min-w-min w-96 h-72 rounded-3xl cursor-pointer flex flex-col shadow-2xl">
                            <img src={TestImage} alt="" className='rounded-t-3xl h-3/5' />
                            <div className="text-center my-5">Take Test</div>
                        </motion.div>
                    </Link>

                    <Link to='/events'>
                        <motion.div
                            whileHover={{ scale: 1.05 }}
                            whileTap={{ scale: 0.95 }}
                            className="m-5 bg-white min-w-min w-96 h-72 rounded-3xl cursor-pointer flex flex-col shadow-2xl">
                            <img src={EventImage} alt="" className='rounded-t-3xl h-3/5' />
                            <div className="text-center my-5">Events</div>
                        </motion.div>
                    </Link>

                </div>
            </motion.div>
        </div>
    )
}

export default SelectionPage
