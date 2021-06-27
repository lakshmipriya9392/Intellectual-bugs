import React, { useState, useEffect } from 'react'
import './../../App.css'
import { Link } from 'react-router-dom'
import { motion } from 'framer-motion'

function testPage() {

    let Question = (props) => {
        return <div className=''>{props.question}</div>
    }

    let Option = (props) => {
        return <div className="text-center text-white bg-black py-2 my-2 text-xl  rounded-3xl  cursor-pointer">{props.answer}</div>
    }
    const optionArr = [
        {
            id: 1,
            option: 'Captcha'
        },
        {
            id: 2,
            option: 'Virtual Assistant'
        },
        {
            id: 3,
            option: 'A robot'
        },
        {
            id: 4,
            option: 'None of these'
        }
    ]


    return (
        <div className='overflow-x-hidden'>
            <motion.div
                initial={{ scale: 0 }}
                animate={{ scale: 1 }}
                transition={{ delay: 1 }}
                className='top-0 right-0 left-0 bottom-0'
            >
                <Link to='/selection'>
                    <div className="absolute top-5 right-10 cursor-pointer text-5xl">&times;</div>
                </Link>

                <div className="  mx-auto mt-24 shadow-2xl w-1/2 h-auto  border-4">
                    <div className="mt-5 mx-16">

                        <div className="flex text-3xl my-2">
                            Q.<Question question="What is a bot ?" />
                        </div>

                        <div className="flex flex-col justify-center mt-2">

                            {optionArr.map((props) => {
                                return (
                                    <Option answer={props.option} key={props.id} />
                                )
                            })}

                            <div className="w-full flex justify-center">
                                <div className="flex float-right mx-0 mr-0 py-10">
                                    <div className="border-2 border-red-500 text-red-500 cursor-pointer mx-5 px-4 py-2 rounded-xl hover:text-white hover:bg-red-500 duration-200">Previous</div>
                                    <div className="border-2 border-blue-500 text-blue-500 cursor-pointer mx-5 px-4 py-2 rounded-xl hover:text-white hover:bg-blue-500 duration-200">Next</div>
                                </div></div>
                        </div>

                    </div>

                </div>
            </motion.div>
        </div>
    )
}

export default testPage
